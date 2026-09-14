using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.DataStorage;
using Core.Application.Info.Reward;
using Core.Application.Quests;
using Core.Application.Models;
using Core.Application.Requirements.Base;
using Core.Application.Requirements.Checkers.SaveState;
using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;
using UnityEngine;
using Zenject;

namespace Core.Application.Models.Quests
{
    public class QuestsModel : IQuestsModel, IInitializable
    {
        public event Action OnCurrentQuestChanged;
        public event Action OnQuestsReset;

        [Inject] private readonly QuestsConfig _questsConfig;
        [Inject] private readonly IDataStorage _dataStorage;
        [Inject] private readonly IRequirementChecker _requirementChecker;
        [Inject] private readonly InventoryModel _inventory;
        [Inject] private readonly IGameEventsBus _gameEventsBus;
        [Inject] private readonly DiContainer _diContainer;
        
        private readonly List<QuestItemModel> _dailyQuests = new();
        private QuestItemModel _currentQuest;
        private readonly Dictionary<Type, object> _eventTypeToRequirementChecker = new();
        
        private QuestsStorageData QuestsData => _dataStorage.Quests;
        public QuestItemModel CurrentQuest => _currentQuest;
        public IReadOnlyList<QuestItemModel> DailyQuests => _dailyQuests;

        
        public void Initialize()
        {
            _dailyQuests.Clear();
            CheckDailyReset();
            LoadDailyQuests();
            LoadCurrentQuest();
            _gameEventsBus.AddListener<UnitDeadEvent>(OnUnitDead);
            _gameEventsBus.AddListener<SpawnUnitEvent>(OnUnitSpawned);
            _gameEventsBus.AddListener<DamageAppliedEvent>(OnDamageApplied);
            _gameEventsBus.AddListener<ResourcesEarnedEvent>(OnResourcesEarned);
        }

        private void OnUnitDead(UnitDeadEvent @event)
        {
            CheckCurrentQuest<UnitDeadEvent, ReqUnitDeadChecker>(@event);
        }

        private void OnUnitSpawned(SpawnUnitEvent @event)
        {
            CheckCurrentQuest<SpawnUnitEvent, ReqUnitSpawnedChecker>(@event);
        }

        private void OnDamageApplied(DamageAppliedEvent @event)
        {
            CheckCurrentQuest<DamageAppliedEvent, ReqDamageAppliedChecker>(@event);
        }

        private void OnResourcesEarned(ResourcesEarnedEvent @event)
        {
            CheckCurrentQuest<ResourcesEarnedEvent, ReqResourcesEarnedChecker>(@event);
        }

        private void CheckCurrentQuest<TEvent, TChecker>(TEvent @event)
            where TChecker : class, IRequirementChecker
        {
            if (_currentQuest == null || _currentQuest.IsCompleted)
            {
                return;
            }
            
            var checker = GetOrCreateChecker<TEvent, TChecker>(@event);
            if (checker.Check(_currentQuest.QuestConfig.Requirement))
            {
                _currentQuest.SetProgress(1);
                Debug.Log($"Quest completed: {_currentQuest.QuestConfig.Id}");
                OnCurrentQuestChanged?.Invoke();
            }
            else
            {
                Debug.Log($"Cur quest progress: {checker.GetProgress(_currentQuest.QuestConfig.Requirement)}");
            }
        }

        private TChecker GetOrCreateChecker<TEvent, TChecker>(TEvent @event)
            where TChecker : class
        {
            var eventType = typeof(TEvent);
            
            if (_eventTypeToRequirementChecker.TryGetValue(eventType, out var checker))
            {
                var typedChecker = (TChecker)checker;
                var updateMethod = typeof(TChecker).GetMethod("UpdateEvent");
                updateMethod?.Invoke(typedChecker, new object[] { @event });
                return typedChecker;
            }
            
            var newChecker = _diContainer.Instantiate<TChecker>(new object[] { @event });
            _eventTypeToRequirementChecker[eventType] = newChecker;
            return newChecker;
        }

        private void CheckDailyReset()
        {
            var todayTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var lastResetTimestamp = QuestsData.GetLastResetTimestamp();

            var todayDate = DateTimeOffset.FromUnixTimeSeconds(todayTimestamp).UtcDateTime.Date;
            var lastResetDate = DateTimeOffset.FromUnixTimeSeconds(lastResetTimestamp).UtcDateTime.Date;

            if (lastResetDate != todayDate)
            {
                ResetDailyQuests();
                QuestsData.SetLastResetTimestamp(todayTimestamp);
                OnQuestsReset?.Invoke();
            }
        }

        private void ResetDailyQuests()
        {
            QuestsData.ClearQuests();
            GenerateDailyQuests();
        }

        private void GenerateDailyQuests()
        {
            var availableQuests = _questsConfig.GetQuests(_dataStorage.CurrentEnemyEpochIndex);
            var random = new System.Random();
            var selectedQuests = availableQuests
                .OrderBy(x => random.Next())
                .Take(_questsConfig.DailyQuestsCount)
                .ToList();

            foreach (var questConfig in selectedQuests)
            {
                QuestsData.SetQuestState(questConfig.Id, QuestState.Inactive);
            }

            QuestsData.SetCurrentQuestIndex(0);
        }

        private void LoadDailyQuests()
        {
            _dailyQuests.Clear();
            var questProgressList = QuestsData.GetAllQuestProgress();

            foreach (var progressData in questProgressList)
            {
                var questConfig = _questsConfig.GetQuests(_dataStorage.CurrentEnemyEpochIndex).FirstOrDefault(q => q.Id == progressData.QuestId);
                if (questConfig != null)
                {
                    var questModel = new QuestItemModel(questConfig, progressData.State);
                    _dailyQuests.Add(questModel);
                }
            }
        }

        private void LoadCurrentQuest()
        {
            var currentIndex = QuestsData.GetCurrentQuestIndex();
            if (currentIndex >= 0 && currentIndex < _dailyQuests.Count)
            {
                _currentQuest = _dailyQuests[currentIndex];
                _currentQuest.SetState(QuestState.Active);
                QuestsData.SetQuestState(_currentQuest.QuestConfig.Id, QuestState.Active);
            }
            else
            {
                _currentQuest = null;
            }
        }

        public void UpdateQuestProgress(string questId, float progress)
        {
            if (_currentQuest == null || _currentQuest.QuestConfig.Id != questId)
            {
                return;
            }

            _currentQuest.SetProgress(progress);

            if (_currentQuest.IsCompleted)
            {
                CompleteCurrentQuest();
            }
        }

        private void CompleteCurrentQuest()
        {
            var reward = _currentQuest.QuestConfig.Reward;
            GiveReward(reward);

            QuestsData.SetQuestState(_currentQuest.QuestConfig.Id, QuestState.Complete);
            MoveToNextQuest();
        }

        private void GiveReward(Reward reward)
        {
            switch (reward.RewardType)
            {
                case RewardType.Resource:
                    _inventory.AddResource(reward.ResourceType, reward.Count);
                    break;
                case RewardType.Item:
                    _inventory.AddItem(reward.ItemType);
                    break;
            }
        }

        private void MoveToNextQuest()
        {
            var currentIndex = QuestsData.GetCurrentQuestIndex();
            var nextIndex = currentIndex + 1;

            if (nextIndex < _dailyQuests.Count)
            {
                QuestsData.SetCurrentQuestIndex(nextIndex);
                LoadCurrentQuest();
                OnCurrentQuestChanged?.Invoke();
            }
            else
            {
                _currentQuest = null;
                OnCurrentQuestChanged?.Invoke();
            }
        }

        public void CheckRequirement()
        {
            if (_currentQuest == null)
            {
                return;
            }

            if (_currentQuest.QuestConfig.Requirement.Check(_requirementChecker))
            {
                UpdateQuestProgress(_currentQuest.QuestConfig.Id, 1f);
            }
        }
    }
}

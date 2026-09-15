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
using UnityEditor;
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
            TryStartNextQuest();
            _gameEventsBus.AddListener<UnitDeadEvent>(OnUnitDead);
            _gameEventsBus.AddListener<SpawnUnitEvent>(OnUnitSpawned);
            _gameEventsBus.AddListener<DamageAppliedEvent>(OnDamageApplied);
            _gameEventsBus.AddListener<ResourcesEarnedEvent>(OnResourcesEarned);
        }

        private void OnUnitDead(UnitDeadEvent @event)
        {
            CheckCurrentQuest<UnitDeadEvent, ReqUnitDeadChecker, ReqUnitDead>(@event);
        }

        private void OnUnitSpawned(SpawnUnitEvent @event)
        {
            CheckCurrentQuest<SpawnUnitEvent, ReqUnitSpawnedChecker, ReqUnitSpawned>(@event);
        }

        private void OnDamageApplied(DamageAppliedEvent @event)
        {
            CheckCurrentQuest<DamageAppliedEvent, ReqDamageAppliedChecker, ReqDamageApplied>(@event);
        }

        private void OnResourcesEarned(ResourcesEarnedEvent @event)
        {
            CheckCurrentQuest<ResourcesEarnedEvent, ReqResourcesEarnedChecker, ReqResourcesEarned>(@event);
        }

        private void CheckCurrentQuest<TEvent, TChecker, TReq> (TEvent @event)
            where TEvent : GameEvent
            where TReq : ReqEvent, IRequirement
            where TChecker : ReqProgressiveChecker<TReq,TEvent>
        {
            if (_currentQuest == null || _currentQuest.IsCompleted)
            {
                return;
            }
            
            var checker = GetOrCreateChecker<TEvent, TChecker, TReq>(@event);
            if (checker.Check(_currentQuest.QuestConfig.Requirement))
            {
                _currentQuest.SetState(QuestState.ReadyToClaim);
                QuestsData.SetQuestState(_currentQuest.QuestConfig.Id, QuestState.ReadyToClaim);
                Debug.Log($"Quest completed: {_currentQuest.QuestConfig.Id}");
            }
            else
            {
                Debug.Log($"Cur quest progress: {checker.GetProgress(_currentQuest.QuestConfig.Requirement)}");
            }
        }

        private TChecker GetOrCreateChecker<TEvent, TChecker, TReq>(TEvent @event)
            where TEvent : GameEvent
            where TReq : ReqEvent, IRequirement
            where TChecker : ReqProgressiveChecker<TReq,TEvent>
        {
            var eventType = typeof(TEvent);
            
            if (_eventTypeToRequirementChecker.TryGetValue(eventType, out var checker))
            {
                var typedChecker = (TChecker)checker;
                typedChecker.UpdateEvent(@event);
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
                    if (progressData.State is QuestState.Active or QuestState.ReadyToClaim)
                    {
                        _currentQuest = questModel;
                    }
                }
            }
        }

        private void TryStartNextQuest()
        {
            if (_dailyQuests.Any(dq => dq.State == QuestState.Active))
            {
                Debug.LogError("Already has active quest");
                return;
            }

            if (!QuestsData.TryGetFirstInactiveQuestId(out var questId))
            {
                Debug.LogError($"Inactive quest not found");
                return;
            }
            
            if (!TryGetQuest(questId, out var questModel))
            {
                Debug.LogError($"Quest with id {questId} not found");
                return;
            }
            questModel.SetState(QuestState.Active);
            QuestsData.SetQuestState(questId, QuestState.Active);
            _currentQuest = questModel;
            
            OnCurrentQuestChanged?.Invoke();
        }

        private void ClaimCurrentQuest()
        {
            if (_currentQuest == null || _currentQuest.State != QuestState.ReadyToClaim)
            {
                return;
            }
            
            var reward = _currentQuest.QuestConfig.Reward;
            GiveReward(reward);

            QuestsData.SetQuestState(_currentQuest.QuestConfig.Id, QuestState.Complete);
            TryStartNextQuest();
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

        public bool TryGetQuest(string questId, out QuestItemModel questModel)
        {
            questModel = _dailyQuests.FirstOrDefault(q => q.QuestConfig.Id == questId);
            return questModel != null; 
        }

    }
}

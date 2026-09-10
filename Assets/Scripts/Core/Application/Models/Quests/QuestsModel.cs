using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.DataStorage;
using Core.Application.Quests;
using Core.Application.Models;
using Core.Application.Requirements.Base;
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

        private readonly List<QuestItemModel> _dailyQuests = new();
        private QuestItemModel _currentQuest;

        
        private QuestsStorageData QuestsData => _dataStorage.Quests;
        public QuestItemModel CurrentQuest => _currentQuest;
        public IReadOnlyList<QuestItemModel> DailyQuests => _dailyQuests;

        
        public void Initialize()
        {
            _dailyQuests.Clear();
            CheckDailyReset();
            LoadDailyQuests();
            LoadCurrentQuest();
        }

        private void CheckDailyReset()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var lastResetDate = QuestsData.GetLastResetDate();

            if (lastResetDate != today)
            {
                ResetDailyQuests();
                QuestsData.SetLastResetDate(today);
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
            var availableQuests = _questsConfig.AvailableQuests.ToList();
            var random = new System.Random();
            var selectedQuests = availableQuests
                .OrderBy(x => random.Next())
                .Take(_questsConfig.DailyQuestsCount)
                .ToList();

            foreach (var questConfig in selectedQuests)
            {
                QuestsData.SetQuestProgress(questConfig.Id, 0f);
                QuestsData.SetQuestCompleted(questConfig.Id, false);
            }

            QuestsData.SetCurrentQuestIndex(0);
        }

        private void LoadDailyQuests()
        {
            _dailyQuests.Clear();
            var questProgressList = QuestsData.GetAllQuestProgress();

            foreach (var progressData in questProgressList)
            {
                var questConfig = _questsConfig.AvailableQuests.FirstOrDefault(q => q.Id == progressData.QuestId);
                if (questConfig != null)
                {
                    var questModel = new QuestItemModel(questConfig, progressData.Progress);
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
            QuestsData.SetQuestProgress(questId, progress);

            if (_currentQuest.IsCompleted)
            {
                CompleteCurrentQuest();
            }
        }

        private void CompleteCurrentQuest()
        {
            var reward = _currentQuest.QuestConfig.Reward;
            GiveReward(reward);

            QuestsData.SetQuestCompleted(_currentQuest.QuestConfig.Id, true);
            MoveToNextQuest();
        }

        private void GiveReward(Core.Application.Info.Shop.Reward reward)
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

using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.Quests;
using Core.Application.Models;
using Unity.Infrastructure.Requirements.Base;
using Zenject;

namespace Core.Application.Models.Quests
{
    public class QuestsModel
    {
        public event Action OnCurrentQuestChanged;
        public event Action OnQuestsReset;

        private readonly QuestsConfig _questsConfig;
        private readonly QuestsStorageData _questsStorageData;
        private readonly IRequirementChecker _requirementChecker;
        private readonly InventoryModel _inventory;

        private List<QuestItemModel> _dailyQuests;
        private QuestItemModel _currentQuest;

        public QuestItemModel CurrentQuest => _currentQuest;
        public IReadOnlyList<QuestItemModel> DailyQuests => _dailyQuests;

        public QuestsModel(
            QuestsConfig questsConfig,
            QuestsStorageData questsStorageData,
            IRequirementChecker requirementChecker,
            InventoryModel inventory)
        {
            _questsConfig = questsConfig;
            _questsStorageData = questsStorageData;
            _requirementChecker = requirementChecker;
            _inventory = inventory;

            _dailyQuests = new List<QuestItemModel>();
            Initialize();
        }

        private void Initialize()
        {
            CheckDailyReset();
            LoadDailyQuests();
            LoadCurrentQuest();
        }

        private void CheckDailyReset()
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var lastResetDate = _questsStorageData.GetLastResetDate();

            if (lastResetDate != today)
            {
                ResetDailyQuests();
                _questsStorageData.SetLastResetDate(today);
                OnQuestsReset?.Invoke();
            }
        }

        private void ResetDailyQuests()
        {
            _questsStorageData.ClearQuests();
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
                _questsStorageData.SetQuestProgress(questConfig.Id, 0f);
                _questsStorageData.SetQuestCompleted(questConfig.Id, false);
            }

            _questsStorageData.SetCurrentQuestIndex(0);
        }

        private void LoadDailyQuests()
        {
            _dailyQuests.Clear();
            var questProgressList = _questsStorageData.GetAllQuestProgress();

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
            var currentIndex = _questsStorageData.GetCurrentQuestIndex();
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
            _questsStorageData.SetQuestProgress(questId, progress);

            if (_currentQuest.IsCompleted)
            {
                CompleteCurrentQuest();
            }
        }

        private void CompleteCurrentQuest()
        {
            var reward = _currentQuest.QuestConfig.Reward;
            GiveReward(reward);

            _questsStorageData.SetQuestCompleted(_currentQuest.QuestConfig.Id, true);
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
            var currentIndex = _questsStorageData.GetCurrentQuestIndex();
            var nextIndex = currentIndex + 1;

            if (nextIndex < _dailyQuests.Count)
            {
                _questsStorageData.SetCurrentQuestIndex(nextIndex);
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

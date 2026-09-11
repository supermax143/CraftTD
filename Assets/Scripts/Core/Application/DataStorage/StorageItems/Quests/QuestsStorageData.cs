using System;
using System.Collections.Generic;
using Core.Application.DataStorage;
using Core.Application.DataStorage.StorageItems;
using Newtonsoft.Json;

namespace Core.Application.Quests
{
    [System.Serializable]
    public class QuestProgressStorageDataInfo
    {
        public List<QuestProgressData> Quests = new();
        public string LastResetDate;
        public int CurrentQuestIndex;
    }

    public class QuestsStorageData
    {
        private const string QUEST_PROGRESS_KEY = "QuestProgress";

        private QuestProgressStorageDataInfo _questProgressInfo = new QuestProgressStorageDataInfo();
        private readonly StringStorageVariable _questProgressVariable;

        public QuestsStorageData(IStorageProvider storageProvider)
        {
            _questProgressVariable = new StringStorageVariable(QUEST_PROGRESS_KEY, storageProvider);

            var progressInfo = LoadProgressInfo();
            if (progressInfo != null)
            {
                _questProgressInfo = progressInfo;
            }
            else
            {
                InitializeDefaultData();
            }
        }

        public bool TryGetQuestProgress(string questId, out QuestProgressData progress)
        {
            progress = _questProgressInfo.Quests.Find(q => q.QuestId == questId);
            return progress != null;
        }

        public IEnumerable<QuestProgressData> GetAllQuestProgress()
        {
            return _questProgressInfo.Quests;
        }

        public void SetQuestState(string questId, QuestState state)
        {
            var existing = _questProgressInfo.Quests.Find(q => q.QuestId == questId);
            if (existing != null)
            {
                existing.State = state;
            }
            else
            {
                _questProgressInfo.Quests.Add(new QuestProgressData { QuestId = questId, State = state });
            }
            Save();
        }


        public void SetCurrentQuestIndex(int index)
        {
            _questProgressInfo.CurrentQuestIndex = index;
            Save();
        }

        public int GetCurrentQuestIndex()
        {
            return _questProgressInfo.CurrentQuestIndex;
        }

        public void SetLastResetDate(string date)
        {
            _questProgressInfo.LastResetDate = date;
            Save();
        }

        public string GetLastResetDate()
        {
            return _questProgressInfo.LastResetDate;
        }

        public void ClearQuests()
        {
            _questProgressInfo.Quests.Clear();
            _questProgressInfo.CurrentQuestIndex = 0;
            Save();
        }

        private void InitializeDefaultData()
        {
            _questProgressInfo = new QuestProgressStorageDataInfo
            {
                Quests = new List<QuestProgressData>(),
                LastResetDate = DateTime.Now.ToString("yyyy-MM-dd"),
                CurrentQuestIndex = 0
            };
        }

        public void Reset()
        {
            InitializeDefaultData();
            Save();
        }

        private void Save()
        {
            _questProgressVariable.Value = SerializeToJson();
        }

        private string SerializeToJson()
        {
            return JsonConvert.SerializeObject(_questProgressInfo, Formatting.Indented);
        }

        private QuestProgressStorageDataInfo LoadProgressInfo()
        {
            var json = _questProgressVariable.Value;
            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<QuestProgressStorageDataInfo>(json);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to load quest progress: {e.Message}");
                return null;
            }
        }
    }
}

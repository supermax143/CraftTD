using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Application.Quests
{
    [CreateAssetMenu(fileName = "QuestsConfig", menuName = "CraftTD/QuestsConfig", order = 1)]
    public class QuestsConfig : ScriptableObject
    {
        [Serializable]
        public struct EpochQuests
        {
            [SerializeField]
            private List<QuestItemConfig> _quests;

            public List<QuestItemConfig> Quests => _quests;
        }
        
        [SerializeField]
        private List<EpochQuests> _epochsQuests = new List<EpochQuests>();
        
        [SerializeField]
        private int _dailyQuestsCount = 3;

        
        public IEnumerable<QuestItemConfig> GetQuests(int epoch)
        {
            if (epoch < 0 || epoch >= _epochsQuests.Count)
            {
                throw new IndexOutOfRangeException("Epoch index is out of range");
            }
            return _epochsQuests[epoch].Quests;
        }
        
        public int DailyQuestsCount => _dailyQuestsCount;
    }
}

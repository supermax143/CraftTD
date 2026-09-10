using System.Collections.Generic;
using UnityEngine;

namespace Core.Application.Quests
{
    [CreateAssetMenu(fileName = "QuestsConfig", menuName = "CraftTD/QuestsConfig", order = 1)]
    public class QuestsConfig : ScriptableObject
    {
        [SerializeField]
        private List<QuestItemConfig> _availableQuests;
        [SerializeField]
        private int _dailyQuestsCount = 3;

        public List<QuestItemConfig> AvailableQuests => _availableQuests;
        public int DailyQuestsCount => _dailyQuestsCount;
    }
}

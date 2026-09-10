using Core.Application.Info.Reward;
using Core.Application.Requirements.Base;
using UnityEngine;

namespace Core.Application.Quests
{
    [CreateAssetMenu(fileName = "QuestItemConfig", menuName = "CraftTD/QuestItemConfig", order = 1)]
    public class QuestItemConfig : ScriptableObject
    {
        [SerializeField]
        private string _id;
        [SerializeField]
        private string _name;
        [SerializeField]
        private string _description;
        [SerializeReference, SubclassSelector]
        private IRequirement _requirement;
        [SerializeField]
        private Reward _reward;

        public string Id => _id;
        public string Name => _name;
        public string Description => _description;
        public IRequirement Requirement => _requirement;
        public Reward Reward => _reward;
    }
}

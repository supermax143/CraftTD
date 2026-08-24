using System.Collections.Generic;
using Core.Application.Spells.Impact;
using UnityEngine;

namespace Core.Application.Spells
{
    public class SpellDefinition : ScriptableObject
    {
        [SerializeField]
        private string _id;
        [SerializeField]
        private string _name;
        [SerializeField]
        private string _description;
        [SerializeField]
        private Sprite _icon;
        [SerializeField]
        private List<SpellUpgrade> _upgrades;
        
        [SerializeField]
        private SpellImpact _impact;

        public string Id => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public int MaxLevel => _upgrades.Count;
        public List<SpellUpgrade> Upgrades => _upgrades;
        public SpellImpact Impact => _impact;
    }
}

using System.Collections.Generic;
using System.Linq;
using Unity.Game;
using UnityEngine;

namespace Unity.Settings
{
    [CreateAssetMenu(fileName = "GameSettings.asset", menuName = "CraftTD/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject, IGameSettings
    {
        [System.Serializable]
        private class FactionColorPair
        {
            public Faction faction;
            public Color color;
        }

        [SerializeField]
        private List<FactionColorPair> _factionColors;
        [SerializeField]
        private float _explosionAnimationTime = 3;
        [SerializeField]
        private float _epochChangeTime = .5f;
        [SerializeField]
        private int _maxEquipedSpells = 2;
        
        
        public float ExplosionAnimationTime => _explosionAnimationTime;

        public float EpochChangeTime => _epochChangeTime;

        public int MaxEquipedSpells => _maxEquipedSpells;

        public bool TryGetFactionColor(Faction faction, out Color color)
        {
            color = default;
            var pair = _factionColors.FirstOrDefault(p => p.faction == faction);
            if (pair == null)
            {
                return false;
            }
            color = pair.color;
            return true;   
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Unity.Game;
using UnityEngine;

namespace Unity.Settings
{
    [CreateAssetMenu(fileName = "GameSettings.asset", menuName = "CraftTD/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [System.Serializable]
        private class FactionColorPair
        {
            public Faction faction;
            public Color color;
        }

        [SerializeField]
        private List<FactionColorPair> _factionColors;


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
using Unity.Game;
using Unity.Infrastructure.GameEvents;
using UnityEngine;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public class ReqUnitsSpawned : ReqProgressive
    {
        [SerializeField]
        private UnitTier _tier;
        [SerializeField]
        private Faction _faction;
        [SerializeField]
        private int _count;
     
        
        public UnitTier Tier => _tier;
        public Faction Faction => _faction;
        public int Count => _count;


        public override string GetProgressSaveIdent() =>
            $"{GameEventTypes.SpawnUnit}_{Tier}_{Faction}_{Count}";
    }
}
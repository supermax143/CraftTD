using Unity.Game;
using UnityEngine;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public class ReqUnitsSpawned : ReqGameEvent
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
        
        
        
    }
}
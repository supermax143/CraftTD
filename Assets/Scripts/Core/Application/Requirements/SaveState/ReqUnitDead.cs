using System;
using Unity.Game;
using Unity.Infrastructure.GameEvents;
using UnityEngine;

namespace Core.Application.Requirements.SaveState
{
    [Serializable]
    public class ReqUnitDead : ReqProgressive
    {
        [SerializeField]
        private UnitTier _tier;
        [SerializeField]
        private bool _anyTier = false;
        [SerializeField]
        private Faction _faction;
        [SerializeField]
        private int _count;
     
        
        public UnitTier Tier => _tier;
        public Faction Faction => _faction;
        public int Count => _count;
        public bool AnyTier => _anyTier;


        public override string GetProgressSaveIdent() =>
            $"{GameEventTypes.UnitDead}_{Tier}_{Faction}_{Count}";
    }
}

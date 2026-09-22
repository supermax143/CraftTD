using System;
using Unity.Game;
using Unity.Infrastructure.GameEvents;
using UnityEngine;

namespace Core.Application.Requirements.SaveState
{
    [Serializable]
    public class ReqUnitSpawned : ReqEvent
    {
        [SerializeField]
        private UnitTier _tier;
        [SerializeField]
        private Faction _faction;
        [SerializeField]
        private int _count;
        [SerializeField]
        private int _epoch = 1;
        
        public UnitTier Tier => _tier;
        public Faction Faction => _faction;
        public int Count => _count;
        public int Epoch => _epoch;


        public override string GetProgressSaveIdent() =>
            $"{GameEventTypes.SpawnUnit}_{Tier}_{Faction}_{Count}";
    }
}
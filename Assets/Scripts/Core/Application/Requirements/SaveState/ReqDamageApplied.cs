using System;
using Unity.Game;
using Unity.Infrastructure.GameEvents;
using UnityEngine;

namespace Core.Application.Requirements.SaveState
{
    [Serializable]
    public class ReqDamageApplied : ReqEvent
    {
        [SerializeField]
        private float _appliedDamage;
        [SerializeField]
        private Faction _faction;
        [SerializeField]
        private int _epoch = 1;
        
        
        public float AppliedDamage => _appliedDamage;
        public Faction Faction => _faction;

        public int Epoch => _epoch;


        public override string GetProgressSaveIdent() =>
            $"{GameEventTypes.DamageApplied}_{Faction}_{AppliedDamage}";
    }
}

using Unity.Game;
using Unity.Infrastructure.GameEvents;
using UnityEngine;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public class ReqDamageApplied : ReqProgressive
    {
        [SerializeField]
        private float _appliedDamage;
        [SerializeField]
        private Faction _faction;
     
        public float AppliedDamage => _appliedDamage;
        public Faction Faction => _faction;

        public override string GetProgressSaveIdent() =>
            $"{GameEventTypes.DamageApplied}_{Faction}_{AppliedDamage}";
    }
}

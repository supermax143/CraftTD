using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;
using UnityEngine;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public class ReqDamageAppliedChecker : ReqProgressiveChecker<ReqDamageApplied, DamageAppliedEvent>
    {
        
        
        public ReqDamageAppliedChecker()
        {
            
        }
        
        public ReqDamageAppliedChecker(DamageAppliedEvent damageAppliedEvent) : base(damageAppliedEvent)
        {
        }

        protected override float GetProgressInternal(ReqDamageApplied req)
        {
            var progress = GetEventProgress(req);
            return Mathf.Clamp01(progress/req.AppliedDamage);
        }

        protected override bool CheckInternal(ReqDamageApplied req)
        {
            if (req.Faction != _event.Faction || req.Epoch != CurrentEpoch)
            {
                return false;
            }
            var progress = GetEventProgress(req);
            progress += (int)_event.AppliedDamage;
            IncrementEventProgress(req, (int)_event.AppliedDamage);
            return req.AppliedDamage <= progress;
        }
    }
}

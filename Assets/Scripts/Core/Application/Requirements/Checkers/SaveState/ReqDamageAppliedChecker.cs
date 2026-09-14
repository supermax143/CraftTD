using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public class ReqDamageAppliedChecker : ReqProgressiveChecker<ReqDamageApplied, DamageAppliedEvent>
    {
        public ReqDamageAppliedChecker(DamageAppliedEvent damageAppliedEvent) : base(damageAppliedEvent)
        {
        }

        protected override bool CheckInternal(ReqDamageApplied req)
        {
            if (req.Faction != _event.Faction)
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

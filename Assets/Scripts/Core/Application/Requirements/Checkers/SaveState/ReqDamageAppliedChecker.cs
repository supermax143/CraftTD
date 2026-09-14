using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public class ReqDamageAppliedChecker : ReqProgressiveChecker<ReqDamageApplied, DamageAppliedEvent>
    {
        public ReqDamageAppliedChecker(DamageAppliedEvent damageAppliedEvent) : base(damageAppliedEvent)
        {
        }

        protected override bool Check(ReqDamageApplied req)
        {
            if (req.Faction != _event.Faction)
            {
                return false;
            }
            var progress = GetProgress(req);
            progress += (int)_event.AppliedDamage;
            IncrementProgress(req, (int)_event.AppliedDamage);
            return req.AppliedDamage <= progress;
        }
    }
}

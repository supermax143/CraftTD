using Unity.Infrastructure.GameEvents;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public class ReqDamageAppliedChecker : ReqProgressiveChecker<ReqDamageApplied>
    {
        private readonly DamageAppliedEvent _damageAppliedEvent;

        public ReqDamageAppliedChecker(DamageAppliedEvent damageAppliedEvent)
        {
            _damageAppliedEvent = damageAppliedEvent;
        }

        protected override bool Check(ReqDamageApplied req)
        {
            if (req.Faction != _damageAppliedEvent.Faction)
            {
                return false;
            }
            var progress = GetProgress(req);
            progress += (int)_damageAppliedEvent.AppliedDamage;
            IncrementProgress(req, (int)_damageAppliedEvent.AppliedDamage);
            return req.AppliedDamage <= progress;
        }
    }
}

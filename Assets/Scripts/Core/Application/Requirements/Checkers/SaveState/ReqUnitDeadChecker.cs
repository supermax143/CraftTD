using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public class ReqUnitDeadChecker : ReqProgressiveChecker<ReqUnitDead>
    {
        private readonly UnitDeadEvent _unitDeadEvent;

        public ReqUnitDeadChecker(UnitDeadEvent unitDeadEvent)
        {
            _unitDeadEvent = unitDeadEvent;
        }

        protected override bool Check(ReqUnitDead req)
        {
            if ((req.Tier != _unitDeadEvent.Tier && !req.AnyTier) || req.Faction != _unitDeadEvent.Faction)
            {
                return false;
            }
            var progress = GetProgress(req);
            progress++;
            IncrementProgress(req, 1);
            return req.Count <= progress;
        }
    }
}

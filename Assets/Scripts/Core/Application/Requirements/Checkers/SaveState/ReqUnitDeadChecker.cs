using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public class ReqUnitDeadChecker : ReqProgressiveChecker<ReqUnitDead, UnitDeadEvent>
    {
        public ReqUnitDeadChecker(UnitDeadEvent unitDeadEvent) : base(unitDeadEvent)
        {
        }

        protected override bool CheckInternal(ReqUnitDead req)
        {
            if ((req.Tier != _event.Tier && !req.AnyTier) || req.Faction != _event.Faction)
            {
                return false;
            }
            var progress = GetEventProgress(req);
            progress++;
            IncrementEventProgress(req, 1);
            return req.Count <= progress;
        }
    }
}

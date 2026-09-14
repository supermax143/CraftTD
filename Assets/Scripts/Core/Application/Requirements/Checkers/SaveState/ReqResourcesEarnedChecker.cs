using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public class ReqResourcesEarnedChecker : ReqProgressiveChecker<ReqResourcesEarned, ResourcesEarnedEvent>
    {
        public ReqResourcesEarnedChecker(ResourcesEarnedEvent resourcesEarnedEvent) : base(resourcesEarnedEvent)
        {
        }

        protected override bool CheckInternal(ReqResourcesEarned req)
        {
            if (req.Resource.Type != _event.Resource.Type)
            {
                return false;
            }
            var progress = GetEventProgress(req);
            progress += _event.Resource.Value;
            IncrementEventProgress(req, _event.Resource.Value);
            return req.Resource.Value <= progress;
        }
    }
}

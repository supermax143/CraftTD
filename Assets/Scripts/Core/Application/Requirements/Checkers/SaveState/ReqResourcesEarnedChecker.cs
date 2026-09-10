using Unity.Infrastructure.GameEvents;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public class ReqResourcesEarnedChecker : ReqProgressiveChecker<ReqResourcesEarned>
    {
        private readonly ResourcesEarnedEvent _resourcesEarnedEvent;

        public ReqResourcesEarnedChecker(ResourcesEarnedEvent resourcesEarnedEvent)
        {
            _resourcesEarnedEvent = resourcesEarnedEvent;
        }

        protected override bool Check(ReqResourcesEarned req)
        {
            if (req.Resource.Type != _resourcesEarnedEvent.Resource.Type)
            {
                return false;
            }
            var progress = GetProgress(req);
            progress += _resourcesEarnedEvent.Resource.Value;
            IncrementProgress(req, _resourcesEarnedEvent.Resource.Value);
            return req.Resource.Value <= progress;
        }
    }
}

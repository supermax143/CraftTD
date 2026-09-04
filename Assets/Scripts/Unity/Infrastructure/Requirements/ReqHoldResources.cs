using Core.Application.Models;

namespace Unity.Infrastructure.Requirements
{
    public record ReqHoldResources : RequirementBase
    {
        public Resource Resource;
    }
}
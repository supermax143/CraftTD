using Core.Application.Models;
using JetBrains.Annotations;
using Zenject;

namespace Unity.Infrastructure.Requirements.Visitors
{
    [UsedImplicitly] //спавнится через zenject в фабрике
    internal class ReqHoldResourcesChecker : RequirementCheckerBase<ReqHoldResources>
    {
        [Inject] private readonly IInventoryModel _inventory;

        protected override bool Check(ReqHoldResources req) 
            => _inventory.HasEnough(req.Resource);

    }
}
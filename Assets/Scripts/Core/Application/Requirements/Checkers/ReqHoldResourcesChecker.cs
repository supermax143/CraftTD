using Core.Application.Models;
using JetBrains.Annotations;
using Zenject;

namespace Core.Application.Requirements.Checkers
{
    [UsedImplicitly] //спавнится через zenject в фабрике
    internal class ReqHoldResourcesChecker : ReqCheckerBase<ReqHoldResources>
    {
        [Inject] private readonly IInventoryModel _inventory;

        protected override bool Check(ReqHoldResources req) 
            => _inventory.HasEnough(req.Resource);

    }
}
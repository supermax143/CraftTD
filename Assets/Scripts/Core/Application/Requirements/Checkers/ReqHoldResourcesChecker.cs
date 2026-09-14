using Core.Application.Models;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Core.Application.Requirements.Checkers
{
    [UsedImplicitly] //спавнится через zenject в фабрике
    internal class ReqHoldResourcesChecker : ReqCheckerBase<ReqHoldResources>
    {
        [Inject] private readonly IInventoryModel _inventory;

        protected override bool CheckInternal(ReqHoldResources req) 
            => _inventory.HasEnough(req.Resource);

        protected override float GetProgressInternal(ReqHoldResources req)
        {
            return Mathf.Clamp01(_inventory.GetResourceCount(req.Resource.Type) / req.Resource.Value);
        }
    }
}
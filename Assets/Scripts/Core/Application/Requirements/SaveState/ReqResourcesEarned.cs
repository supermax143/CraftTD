using Core.Application.Models;
using Unity.Infrastructure.GameEvents;
using UnityEngine;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public class ReqResourcesEarned : ReqProgressive
    {
        [SerializeField]
        private Resource _resource;
     
        public Resource Resource => _resource;

        public override string GetProgressSaveIdent() =>
            $"{GameEventTypes.ResourcesEarned}_{Resource.Type}_{Resource.Value}";
    }
}

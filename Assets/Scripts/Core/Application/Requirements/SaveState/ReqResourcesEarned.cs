using System;
using Core.Application.Models;
using Unity.Infrastructure.GameEvents;
using UnityEngine;

namespace Core.Application.Requirements.SaveState
{
    [Serializable]
    public class ReqResourcesEarned : ReqEvent
    {
        [SerializeField]
        private Resource _resource;
     
        public Resource Resource => _resource;

        public override string GetProgressSaveIdent() =>
            $"{GameEventTypes.ResourcesEarned}_{Resource.Type}";
    }
}

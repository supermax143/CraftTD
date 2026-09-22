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
        [SerializeField]
        private int _epoch = 1;
        
        public Resource Resource => _resource;

        public int Epoch => _epoch;

        public override string GetProgressSaveIdent() =>
            $"{GameEventTypes.ResourcesEarned}_{Resource.Type}";
    }
}

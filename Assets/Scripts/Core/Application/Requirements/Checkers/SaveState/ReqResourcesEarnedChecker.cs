using System;
using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;
using Unity.Mathematics;
using UnityEngine;
using Math = Unity.Mathematics.Geometry.Math;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public class ReqResourcesEarnedChecker : ReqProgressiveChecker<ReqResourcesEarned, ResourcesEarnedEvent>
    {

        public ReqResourcesEarnedChecker()
        {
            
        }
        
        public ReqResourcesEarnedChecker(ResourcesEarnedEvent resourcesEarnedEvent) : base(resourcesEarnedEvent)
        {
        }

        protected override float GetProgressInternal(ReqResourcesEarned req)
        {
            var progress = GetEventProgress(req);
            return Mathf.Clamp01((float)progress/(float)req.Resource.Value);
        }

        protected override bool CheckInternal(ReqResourcesEarned req)
        {
            if (req.Resource.Type != _event.Resource.Type || req.Epoch != CurrentEpoch)
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

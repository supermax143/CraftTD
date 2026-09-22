using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;
using UnityEngine;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public class ReqUnitSpawnedChecker : ReqProgressiveChecker<ReqUnitSpawned, SpawnUnitEvent>
    {

        public ReqUnitSpawnedChecker()
        {
            
        }
        
        public ReqUnitSpawnedChecker(SpawnUnitEvent spawnUnitEvent) : base(spawnUnitEvent)
        {
        }

        protected override float GetProgressInternal(ReqUnitSpawned req)
        {
            var progress = GetEventProgress(req);
            return Mathf.Clamp01((float)progress/(float)req.Count);
        }

        protected override bool CheckInternal(ReqUnitSpawned req)
        {
            if (req.Tier != _event.Tier 
                || req.Faction != _event.Faction
                || req.Epoch != CurrentEpoch)
            {
                return false;
            }
            var progress = GetEventProgress(req);
            progress++;
            IncrementEventProgress(req, 1);
            return req.Count <= progress;
        }
    }
}
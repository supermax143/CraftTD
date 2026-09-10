using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public class ReqUnitSpawnedChecker : ReqProgressiveChecker<ReqUnitSpawned>
    {
        private readonly SpawnUnitEvent _spawnUnitEvent;

        public ReqUnitSpawnedChecker(SpawnUnitEvent spawnUnitEvent)
        {
            _spawnUnitEvent = spawnUnitEvent;
        }

        protected override bool Check(ReqUnitSpawned req)
        {
            if (req.Tier != _spawnUnitEvent.Tier || req.Faction != _spawnUnitEvent.Faction)
            {
                return false;
            }
            var progress = GetProgress(req);
            progress++;
            IncrementProgress(req, 1);
            return req.Count <= progress;
        }
    }
}
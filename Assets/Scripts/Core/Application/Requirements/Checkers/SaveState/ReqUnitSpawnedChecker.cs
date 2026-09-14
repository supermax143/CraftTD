using Core.Application.Requirements.SaveState;
using Unity.Infrastructure.GameEvents;

namespace Core.Application.Requirements.Checkers.SaveState
{
    public class ReqUnitSpawnedChecker : ReqProgressiveChecker<ReqUnitSpawned, SpawnUnitEvent>
    {
        public ReqUnitSpawnedChecker(SpawnUnitEvent spawnUnitEvent) : base(spawnUnitEvent)
        {
        }

        protected override bool Check(ReqUnitSpawned req)
        {
            if (req.Tier != _event.Tier || req.Faction != _event.Faction)
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
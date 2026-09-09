using Unity.Infrastructure.GameEvents;

namespace Unity.Infrastructure.Requirements.Visitors
{
    public class ReqUnitsSpawnedChecker : RequirementCheckerBase<ReqUnitsSpawned>
    {
        private readonly SpawnUnitEvent _spawnUnitEvent;

        public ReqUnitsSpawnedChecker(SpawnUnitEvent spawnUnitEvent)
        {
            _spawnUnitEvent = spawnUnitEvent;
        }

        protected override bool Check(ReqUnitsSpawned req)
        {
            if (req.Tier != _spawnUnitEvent.Tier || req.Faction != _spawnUnitEvent.Faction)
            {
                return false;
            }

            return true;
        }
    }
}
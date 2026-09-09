using Unity.Game;

namespace Unity.Infrastructure.GameEvents
{
    public class SpawnUnit : GameEvent
    {
        public UnitTier Tier { get; }
        public int EpochId { get; }

        public SpawnUnit(UnitTier tier, int epochId)
        {
            Tier = tier;
            EpochId = epochId;
            
            Name = GameEventTypes.Scene;
            _params[nameof(tier)] = tier.ToString();
            _params[nameof(epochId)] = epochId.ToString();
        }

    }
}
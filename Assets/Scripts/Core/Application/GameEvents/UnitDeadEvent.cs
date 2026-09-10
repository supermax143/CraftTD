using Core.Application.Models;
using Unity.Game;

namespace Unity.Infrastructure.GameEvents
{
    public class UnitDeadEvent : GameEvent
    {
        public UnitTier Tier { get; }
        public int EpochId { get; }
        public Faction Faction { get; }

        
        public UnitDeadEvent(UnitModel unit) : this(unit.Info.Tier, unit.Faction, unit.EpochId)
        {
            
        }
        
        public UnitDeadEvent(UnitTier tier, Faction faction, int epochId)
        {
            Tier = tier;
            EpochId = epochId;
            Faction = faction;
            
            Name = GameEventTypes.UnitDead;
            _params[nameof(tier)] = tier.ToString();
            _params[nameof(epochId)] = epochId.ToString();
            _params[nameof(faction)] = faction.ToString();
        }

    }
}

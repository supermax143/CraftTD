using Unity.Game.Attributes.Specific;

namespace Unity.Game.Entity
{
    public class TowerEntity : GameEntityData
    {
        
        private HealthAttribute _health = new();
        private readonly Faction _faction;

        public Faction Faction => _faction;
        
        public TowerEntity(Faction faction, uint epoch, GameStats gameStats)
        {
            _faction = faction;
            _health = new HealthAttribute(gameStats.GetTowerHealth(epoch, faction));
        }

    }
}
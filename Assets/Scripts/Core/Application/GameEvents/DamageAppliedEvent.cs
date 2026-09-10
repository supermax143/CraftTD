using Unity.Game;

namespace Unity.Infrastructure.GameEvents
{
    public class DamageAppliedEvent : GameEvent
    {
        public float AppliedDamage { get; }
        public Faction Faction { get; }

        public DamageAppliedEvent(float appliedDamage, Faction faction)
        {
            AppliedDamage = appliedDamage;
            Faction = faction;
            
            Name = GameEventTypes.DamageApplied;
            _params[nameof(appliedDamage)] = appliedDamage.ToString();
            _params[nameof(faction)] = faction.ToString();
        }
    }
}

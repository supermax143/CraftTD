using UnityEngine;

namespace Unity.Game
{
    public class AttackTarget : MonoBehaviour
    {
        [SerializeField]
        private TargetType _targetType;
        
        private HealthComponent _health;
        
        private Faction _faction;

        public Faction Faction => _faction;

        public HealthComponent Health => _health;

        public TargetType Type => _targetType;

        public void SetFaction(Faction faction)
        {
            _faction = faction;
        }

        public void Initialize(HealthComponent health)
        {
            _health = health;
        }
    }
}
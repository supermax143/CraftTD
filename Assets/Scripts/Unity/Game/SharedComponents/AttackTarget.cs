using UnityEngine;

namespace Unity.Game
{
    public class AttackTarget : MonoBehaviour
    {
        [SerializeField]
        private TargetType _targetType;
        [SerializeField, HideInInspector]
        private Collider _collider;
        
        private HealthComponent _health;
        
        private Faction _faction;

        public Faction Faction => _faction;

        public HealthComponent Health => _health;

        public TargetType Type => _targetType;

        
        private void OnValidate()
        {
            _collider = GetComponent<Collider>();
        }
        
        public void SetFaction(Faction faction)
        {
            _faction = faction;
        }

        public void Initialize(HealthComponent health)
        {
            _health = health;
        }
        
        public Vector3 GetClosestPosition(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }
    }
}
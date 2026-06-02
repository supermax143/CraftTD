using System;
using UnityEngine;

namespace Unity.Game
{
    public class AttackTarget : MonoBehaviour
    {
        public event Action OnDeath;
        
        [SerializeField]
        private TargetType _targetType;
        [SerializeField, HideInInspector]
        private Collider _collider;
        
        private HealthComponent _health;
        
        private Faction _faction;

        public Faction Faction => _faction;

        public HealthComponent Health => _health;

        public TargetType Type => _targetType;

        public bool IsDead => _health.IsDead;
        
        private void OnValidate()
        {
            _collider = GetComponent<Collider>();
        }

        private void DeathHandler()
        {
            OnDeath?.Invoke();
        }
        
        public void SetFaction(Faction faction)
        {
            _faction = faction;
        }

        public void Initialize(HealthComponent health)
        {
            _health = health;
            _health.OnDeath += DeathHandler;
        }
        
        public Vector3 GetClosestPosition(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }
        
        private void OnDestroy()
        {
            _health.OnDeath -= DeathHandler;
        }
    }
}
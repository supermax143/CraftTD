using System;
using UnityEngine;

namespace Unity.Game
{
    public abstract class AttackTargetBase : MonoBehaviour
    {
        public event Action OnDeath;
        
        [SerializeField]
        private TargetType _targetType;
        [SerializeField]
        private Transform _attackTransform;
        
        
        public HealthComponent HealthComponent => _healthComponent;
        public TargetType Type => _targetType;

        public bool IsDead => _healthComponent.IsDead;
        public Faction Faction => _faction;

        public Vector3 LastAttackDirection => _lastAttackDirection;

        private HealthComponent _healthComponent;
        
        private Faction _faction;
        private Vector3 _lastAttackDirection;

        public abstract bool TryGetClosestPosition(Vector3 position, out Vector3 closestPosition);

        public bool TryGetAttackPosition(bool top,out Vector3 pos)
        {
            pos = default;
            if (!top)
            {
                pos = transform.position;
                return  true;
            }
            
            if (_attackTransform == null)
            {
                return false;
            }
            pos = _attackTransform.position;
            return true;
        }
        
        public void SetFaction(Faction faction)
        {
            _faction = faction;
        }

        public void Initialize(HealthComponent health)
        {
            _healthComponent = health;
            _healthComponent.OnDeath += DeathHandler;
        }
        
        private void DeathHandler()
        {
            OnDeath?.Invoke();
        }

        
        private void OnDestroy()
        {
            if (_healthComponent != null)
            {
                _healthComponent.OnDeath -= DeathHandler;
            }
        }

        public void SetLastAttackDirection(Vector3 direction)
        {
            _lastAttackDirection = direction;
        }
    }
    
    public abstract class AttackTargetBase<TCollider> : AttackTargetBase
        where TCollider : Component
    {
        
        [SerializeField, HideInInspector]
        protected TCollider _collider;
        

        
        private void OnValidate()
        {
            _collider = GetComponent<TCollider>();
        }

    }
}
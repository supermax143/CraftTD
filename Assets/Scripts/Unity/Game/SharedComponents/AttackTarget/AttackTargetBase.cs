using System;
using UnityEngine;

namespace Unity.Game
{
    public abstract class AttackTargetBase : MonoBehaviour
    {
        public event Action OnDeath;
        
        [SerializeField]
        private TargetType _targetType;
        
        public HealthComponent HealthComponent => _healthComponent;
        public TargetType Type => _targetType;

        public bool IsDead => _healthComponent.IsDead;
        public Faction Faction => _faction;
        
        private HealthComponent _healthComponent;
        
        private Faction _faction;
        
        public abstract Vector3 GetClosestPosition(Vector3 position);

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
            _healthComponent.OnDeath -= DeathHandler;
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

        /*private void DeathHandler()
        {
            OnDeath?.Invoke();
        }*/
        
        /*public void SetFaction(Faction faction)
        {
            _faction = faction;
        }

        public void Initialize(HealthComponent health)
        {
            _healthComponent = health;
            _healthComponent.OnDeath += DeathHandler;
        }
        
        public Vector3 GetClosestPosition(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }*/
        
        /*private void OnDestroy()
        {
            _healthComponent.OnDeath -= DeathHandler;
        }*/
    }
}
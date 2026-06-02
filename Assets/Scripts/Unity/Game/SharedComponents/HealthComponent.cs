using System;
using UnityEngine;

namespace Unity.Game
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action OnDamage;
        public event Action OnDeath;
        
        [SerializeField]
        private float _maxHealth;
        
        private float _currentHealth;
        
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public bool IsDead => _currentHealth <= 0;

        public void Initialize()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                OnDeath?.Invoke();
            }
            OnDamage?.Invoke();
            Debug.Log($"Health: {_currentHealth} / {_maxHealth}");
        }
        
    }
}
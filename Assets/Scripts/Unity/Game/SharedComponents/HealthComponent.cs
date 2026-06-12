using System;
using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{
    public class HealthComponent : GameComponent
    {
        public event Action OnDamage;
        public event Action OnDeath;
        
        
        [SerializeField]
        private HealthAttribute _health = new HealthAttribute(10);
        
        private float _currentHealth;
        
        public float MaxHealth => _health.ValueModified;
        public float CurrentHealth => _currentHealth;
        public bool IsDead => _currentHealth <= 0;

        public override void SetData(GameEntityData data)
        {
            base.SetData(data);
            _currentHealth = MaxHealth;
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
            Debug.Log($"Health: {_currentHealth} / {MaxHealth}");
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 position = transform.position + Vector3.up * 2f;
            Color healthColor = Color.white;
            UnityEditor.Handles.Label(position, $"h: {_currentHealth:F1} / {MaxHealth:F1}", new GUIStyle
            {
                normal = { textColor = healthColor },
                fontSize = 12,
                fontStyle = FontStyle.Bold
            });
        }
#endif

    }
}
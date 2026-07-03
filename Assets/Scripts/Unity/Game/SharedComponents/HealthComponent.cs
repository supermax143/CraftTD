using System;
using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;
using Unity.Utils;
using UnityEngine;

namespace Unity.Game
{
    public class HealthComponent : GameComponent
    {
        public event Action<int> OnDamage;
        public event Action OnDeath;
        
        [SerializeField]
        private SpriteProgressbar _progressBar;
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
            _progressBar.Initialize(MaxHealth, MaxHealth);
            _progressBar.gameObject.SetActive(false);
        }

        public void TakeDamage(float damage)
        {
            if (damage > _currentHealth)
            {
                damage = _currentHealth;
            }
            
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                OnDeath?.Invoke();
            }
            OnDamage?.Invoke((int)damage);
            _progressBar.SetValue(_currentHealth);
            _progressBar.gameObject.SetActive(_currentHealth > 0);
            Debug.Log($"Health: {_currentHealth} / {MaxHealth}");
        }

        public void HideHealth()
        {
            _progressBar.gameObject.SetActive(false);
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
using System;
using Core.Application.Models;
using Unity.Game.Attributes.Specific;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Unity.Game
{
    /// <summary>
    /// Компонент для управления наградами за нанесение урона юниту
    /// Награда начисляется пропорционально нанесенному урону относительно максимального здоровья
    /// </summary>
    public class RewardForDamageComponent : GameComponent
    {
        [SerializeField]
        private RewardMoneyAttribute _rewardMoney = new RewardMoneyAttribute(0);
        [SerializeField] 
        private Vector2 _dropDirection;
        [SerializeField] 
        private Transform _dropTransform;
        
        [Inject] private ILevelRewardAggregator _rewardAggregator;
        [Inject] private DropManager _dropManager;
        
        private HealthComponent _healthComponent;

        public int RewardMoney => _rewardMoney.BaseValueModified;

        public void Initialize(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
            _healthComponent.OnDamage += OnDamageHandler;
        }

        private void OnDamageHandler(int damage)
        {
            float rewardRatio = damage / _healthComponent.MaxHealth;
            int reward = Mathf.RoundToInt(rewardRatio * RewardMoney);
            var direction = _dropDirection;
            
            float angle = Random.Range(0f, 360f);
            direction.y = Mathf.Sin(angle * Mathf.Deg2Rad);
            if (reward > 0)
            {
                _rewardAggregator.AddMoney(reward);
                _dropManager.ShowDrop(new Resource(ResourceType.Money, reward), _dropTransform.position, direction);
            }
        }
        
        private void OnDestroy()
        {
            if (_healthComponent != null)
            {
                _healthComponent.OnDamage -= OnDamageHandler;
            }
        }
    }
}

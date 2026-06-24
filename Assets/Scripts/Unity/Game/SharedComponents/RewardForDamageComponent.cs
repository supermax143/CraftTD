using System;
using Core.Application.Models;
using Unity.Game.Attributes.Specific;
using UnityEngine;
using Zenject;

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
        
        
        [Inject] private ILevelRewardAggregator _rewardAggregator;
        [Inject] private DropManager _dropManager;
        
        private HealthComponent _healthComponent;

        public int RewardMoney => _rewardMoney.ValueModified;

        public void Initialize(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
            _healthComponent.OnDamage += OnDamageHandler;
        }

        private void OnDamageHandler(int damage)
        {
            float rewardRatio = damage / _healthComponent.MaxHealth;
            int reward = Mathf.RoundToInt(rewardRatio * RewardMoney);
            
            if (reward > 0)
            {
                _rewardAggregator.AddMoney(reward);
                _dropManager.ShowDrop(new Resource(ResourceType.Money, reward), transform.position, _dropDirection);
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

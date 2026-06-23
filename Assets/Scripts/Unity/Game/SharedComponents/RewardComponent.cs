using System;
using DG.Tweening;
using Unity.Game.Attributes.Specific;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Unity.Game
{
    /// <summary>
    /// Компонент для управления наградами за убийство юнита
    /// </summary>
    public class RewardComponent : GameComponent
    {
        [SerializeField]
        private RewardMoneyAttribute _rewardMoney = new RewardMoneyAttribute(0);
        
        [Inject] ILevelRewardAggregator _rewardAggregator;
        
        
        private HealthComponent _healthComponent;

        public int RewardMoney => _rewardMoney.ValueModified;

        public void Initialize(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
            _healthComponent.OnDeath += OnDeathHandler;
        }

        private void OnDeathHandler()
        {
            _rewardAggregator.AddMoney(RewardMoney);
        }

       
        
        
        private void OnDestroy()
        {
            _healthComponent.OnDeath -= OnDeathHandler;
        }
    }
}

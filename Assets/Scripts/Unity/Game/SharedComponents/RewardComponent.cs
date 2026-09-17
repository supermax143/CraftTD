using System;
using Core.Application.Models;
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
        [Inject] DropManager _dropManager;
        
        private HealthComponent _healthComponent;

        public int RewardMoney => _rewardMoney.BaseValueModified;

        public void Initialize(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
        }

        public void OnDeathHandler()
        {
            _rewardAggregator.AddMoney(RewardMoney);
            _dropManager.ShowSceneDrop(new Resource(ResourceType.Money, RewardMoney), transform.position);
        }
        
        private void OnDestroy()
        {
            if (_healthComponent == null)
            {
                return;
            }
        }
    }
}

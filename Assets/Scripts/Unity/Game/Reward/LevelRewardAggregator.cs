using System;
using Core.Application.Models;
using Unity.Game.Attributes.Specific;
using Unity.Infrastructure.GameEvents;
using Unity.U2D.Physics;
using Zenject;
using IInitializable = Unity.VisualScripting.IInitializable;

namespace Unity.Game
{
    public class LevelRewardAggregator : ILevelRewardAggregator, IInitializable
    {

        public event Action OnMoneyChanged;
        
        [Inject] private IInventoryModel _inventoryModel;
        [Inject] private IGameEventsBus _gameEventsBus;

        private RewardMoneyAttribute _moneyReward = new(0);
        
        public Resource Money => new Resource(ResourceType.Money, _moneyReward.BaseValueModified);
        
        
        public void Initialize()
        {
            _moneyReward.BaseValue = 0;
        }
        
        
        public void AddMoney(int amount)
        {
            _moneyReward.BaseValue += amount;
            OnMoneyChanged?.Invoke();
            _gameEventsBus.TriggerEvent(new ResourcesEarnedEvent(Money));
        }

        public void HandleBattleFinish()
        {
            foreach (var item in _inventoryModel.Items)
            {
                foreach (var modifier in item.Modifiers)
                {
                    _moneyReward.AddModifier(modifier);
                }
            }
        }
        
        public void Reset()
        {
            _moneyReward.BaseValue = 0;
            _moneyReward.ClearModifiers();
        }

       
    }
}
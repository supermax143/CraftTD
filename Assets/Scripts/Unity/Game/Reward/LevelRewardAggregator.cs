using System;
using Core.Application.Models;
using Unity.Game.Attributes.Specific;
using Unity.U2D.Physics;
using Zenject;
using IInitializable = Unity.VisualScripting.IInitializable;

namespace Unity.Game
{
    public class LevelRewardAggregator : ILevelRewardAggregator, IInitializable
    {

        public event Action OnMoneyChanged;
        
        [Inject]private IInventoryModel _inventoryModel;
        
        // private Resource _money = Resource.Money(0);
        //
        // public Resource Money => _money;

        private RewardMoneyAttribute _moneyReward = new(0);
        
        public Resource Money => new Resource(ResourceType.Money, _moneyReward.BaseBaseValueModified);
        
        
        public void Initialize()
        {
            _moneyReward.BaseValue = 0;
        }
        
        
        public void AddMoney(int amount)
        {
            _moneyReward.BaseValue += amount;
            OnMoneyChanged?.Invoke();
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
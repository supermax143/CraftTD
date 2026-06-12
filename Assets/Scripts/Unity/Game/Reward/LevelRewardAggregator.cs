using System;
using Core.Application.Models;

namespace Unity.Game
{
    public class LevelRewardAggregator : ILevelRewardAggregator
    {

        public event Action OnMoneyChanged;
        
        private Resource _money = Resource.Money(0);

        public Resource Money => _money;

        public void AddMoney(int amount)
        {
            _money += Resource.Money(amount);
            OnMoneyChanged?.Invoke();
        }
        
    }
}
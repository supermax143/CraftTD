using System;
using Core.Application.Models;
using Unity.U2D.Physics;

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

        public void Reset()
        {
            _money.Value = 0;
        }
    }
}
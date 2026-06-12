using System;

namespace Unity.Game
{
    public class LevelRewardAggregator : ILevelRewardAggregator
    {

        public event Action OnMoneyChanged;
        
        private uint _money;

        public uint Money => _money;

        public void AddMoney(int amount)
        {
            _money += (uint)amount;
            OnMoneyChanged?.Invoke();
        }
        
    }
}
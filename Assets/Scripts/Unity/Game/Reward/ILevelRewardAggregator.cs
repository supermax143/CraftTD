using System;

namespace Unity.Game
{
    public interface ILevelRewardAggregator
    {
        void AddMoney(int amount);
        uint Money { get; }
        event Action OnMoneyChanged;
    }
}
using System;
using Core.Application.Models;

namespace Unity.Game
{
    public interface ILevelRewardAggregator
    {
        void AddMoney(int amount);
        Resource Money { get; }
        event Action OnMoneyChanged;
        void Reset();
    }
}
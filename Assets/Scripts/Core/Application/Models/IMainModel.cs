
using System;

namespace Core.Application.Models
{
    public interface IMainModel 
    {
        EpochModel EnemyEpoch { get; }
        EpochModel PlayerEpoch { get; }
        InventoryModel Inventory { get; }
        Resource Money { get; set; }
        int CurrentPlayerEpochNumber { get; }
        int CurrentEnemyEpochNumber { get; }
        int SelectedEnemyEpochIndex { get; }
        bool HasNextEpoch();
        void IncreaseEnemyEpoch();
        void CompleteEpochForMoney();
        int GetEpochCompleteCost();
        void Reset();
        void SelectEnemyEpochIndex(int index);
        void CompleteEpoch();
        event Action OnEnemyEpochChanged;
        event Action OnPlayerEpochChanged;
    }
    
    internal interface IMainModelInternal : IMainModel
    {
    }
    
}
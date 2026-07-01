using Core.Application.DataStorage.StorageItems;
using Core.Application.Interfaces;
using Core.Application.Models;

namespace Core.Application.DataStorage
{
    public interface IDataStorage : IBootstrapStep
    {
        TutorialStorageData TutorialStorage { get; }
        PurchasesStorageData Purchases { get; }
        InventoryStorageData Inventory { get; }
        int CurrentPlayerEpochIndex { get; }
        int CurrentEnemyEpochIndex { get; }
        void SetCurrentPlayerEpoch(int index);
        void Reset();
    }
}
using Core.Application.DataStorage.StorageItems;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Application.Spells;

namespace Core.Application.DataStorage
{
    public interface IDataStorage : IBootstrapStep
    {
        TutorialStorageData TutorialStorage { get; }
        PurchasesStorageData Purchases { get; }
        InventoryStorageData Inventory { get; }
        int CurrentPlayerEpochIndex { get; }
        int CurrentEnemyEpochIndex { get; }
        SpellProgressStorageData Spells { get; }
        void SetCurrentPlayerEpoch(int index);
        void Reset();
    }
}
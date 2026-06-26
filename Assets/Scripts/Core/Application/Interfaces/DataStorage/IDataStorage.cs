using Core.Application.DataStorage.StorageItems;
using Core.Application.Interfaces;
using Core.Application.Models;

namespace Core.Application.DataStorage
{
    public interface IDataStorage : IBootstrapStep
    {
        TutorialStorageData TutorialStorage { get; }
        PurchasesStorageData Purchases { get; }
        Resource UserMoney { get; }
        int CurrentPlayerEpochIndex { get; }
        void AddMoney(int amount);
        void SetCurrentPlayerEpoch(int index);
        void Reset();
    }
}
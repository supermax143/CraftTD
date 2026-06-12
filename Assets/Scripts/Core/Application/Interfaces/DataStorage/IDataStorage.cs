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
        int CurrentEpochIndex { get; }
        void AddMoney(int amount);
        void SetCurrentEpoch(int index);
        void Reset();
    }
}
using Core.Application.DataStorage.StorageItems;
using Core.Application.Interfaces;

namespace Core.Application.DataStorage
{
    public interface IDataStorage : IBootstrapStep
    {
        TutorialStorageData TutorialStorage { get; }
        PurchasesStorageData Purchases { get; }
        uint UserMoney { get; }
        int CurrentEpochIndex { get; }
        void AddMoney(uint Value);
        void SetCurrentEpoch(int index);
    }
}
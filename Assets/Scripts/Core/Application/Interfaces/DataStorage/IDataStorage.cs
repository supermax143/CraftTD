using Core.Application.DataStorage.StorageItems;
using Core.Application.Interfaces;

namespace Core.Application.DataStorage
{
    public interface IDataStorage : IBootstrapStep
    {
        TutorialStorageData TutorialStorage { get; }
        PurchasesStorageData Purchases { get; }
        uint UserMoney { get; }
        void AddMoney(uint Value);
        void SetMoney(uint Value);
    }
}
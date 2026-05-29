using System;
using System.Threading.Tasks;
using Core.Application.DataStorage.StorageItems;
using UnityEngine;
using Zenject;

namespace Core.Application.DataStorage
{

    /// <summary>
    /// Main data storage controller that manages game data persistence.
    /// Uses typed storage variables with configurable storage providers for type-safe data access.
    /// Maintains backward compatibility with existing PlayerPrefs-based data.
    /// </summary>
    internal class DataStorage : IDataStorage 
    {
       
        /// <summary>
        /// Event triggered when data is updated.
        /// </summary>
        public event Action TriggerUpdate;
        
        
        [Inject]
        private ILocalStorageProvider _localStorageProvider;
        [Inject]
        private IGlobalStorageProvider _globalStorageProvider;
        
        private UserStorageData _userStorageData;
        private TutorialStorageData _tutorialStorageData;
        private PurchasesStorageData _purchasesStorageData;
       

        public TutorialStorageData TutorialStorage => _tutorialStorageData;
        public PurchasesStorageData Purchases => _purchasesStorageData;
        
       
        public Task Initialize()
        {
            _tutorialStorageData = new TutorialStorageData(_localStorageProvider);
            _userStorageData = new UserStorageData(_globalStorageProvider);
            
            Debug.Log($"{this.GetType().Name} Initialized");
            return Task.CompletedTask;
        }


        public void Reset()
        {
            _localStorageProvider.Reset();
            _globalStorageProvider.Reset();
            
            _tutorialStorageData.Reset();
            _userStorageData.Reset();
            _purchasesStorageData.Reset();
            
            TriggerUpdate?.Invoke();
        }

        public void AddMoney(uint Value)
        {
            _userStorageData.AddMoney(Value);
            TriggerUpdate?.Invoke();
        }
        
        public void SetMoney(uint Value)
        {
            _userStorageData.SetMoney(Value);
            TriggerUpdate?.Invoke();
        }

        public uint UserMoney
        {
            get { return _userStorageData.Money; }
        }
        
        public void AddPurchase(string id)
        {
            _purchasesStorageData.AddPurchase(id);
        }

        public bool HasPurchases()
        {
            return _purchasesStorageData.HasAnyPurchases();
        }

        public int GetPurchase(string id)
        {
            return _purchasesStorageData.GetPurchase(id);
        }

    }
}

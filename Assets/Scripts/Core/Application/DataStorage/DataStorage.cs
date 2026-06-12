using System;
using System.Threading.Tasks;
using Core.Application.DataStorage.StorageItems;
using Core.Application.Interfaces;
using Core.Application.Models;
using UnityEngine;
using Zenject;

namespace Core.Application.DataStorage
{


    
#if DEBUG_MODE
    internal class DataStorage : IDataStorage, IInitializable
#else
    internal class DataStorage : IDataStorage, IBootstrapStep
#endif
    {
       
        
        [Inject]
        private ILocalStorageProvider _localStorageProvider;
        [Inject]
        private IGlobalStorageProvider _globalStorageProvider;
        
        private TutorialStorageData _tutorialStorageData;
        private PurchasesStorageData _purchasesStorageData;
        private IntStorageVariable _curEpochIndex;
        private EpochStorageData _epochData;

        public TutorialStorageData TutorialStorage => _tutorialStorageData;
        public PurchasesStorageData Purchases => _purchasesStorageData;
        public int CurrentEpochIndex => _curEpochIndex.Value;
        
#if DEBUG_MODE
        public void Initialize()
        {
            Init();
        }
#endif
        
        public Task Init()
        {
            _tutorialStorageData = new TutorialStorageData(_localStorageProvider);
            _epochData = new EpochStorageData(_localStorageProvider);
            _curEpochIndex = new IntStorageVariable("CurrentEpoch", _localStorageProvider, 0);
            _purchasesStorageData = new PurchasesStorageData(_localStorageProvider);
            Debug.Log($"{this.GetType().Name} Initialized");
            return Task.CompletedTask;
        }


        public void Reset()
        {
            _localStorageProvider.Reset();
            _globalStorageProvider.Reset();
            
            _tutorialStorageData.Reset();
            _curEpochIndex.Value = 0;
            _epochData.Reset();
            _purchasesStorageData.Reset();
        }

        internal void SetEpochIndex(int index)
        {
            _curEpochIndex.Value = index;
            _epochData.Reset();
        }
        
        public void AddMoney(int amount)
        {
            _epochData.AddMoney(amount);
        }

        public void SetMoney(Resource money)
        {
            _epochData.Money = money;
        }

        public void SetCurrentEpoch(int index)
        {
            _curEpochIndex.Value = index;
        }


        public Resource UserMoney
        {
            get { return _epochData.Money; }
        }

        internal EpochStorageData EpochData => _epochData;


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

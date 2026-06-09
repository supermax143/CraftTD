using System;
using System.Threading.Tasks;
using Core.Application.DataStorage.StorageItems;
using Core.Application.Interfaces;
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
        private FloatStorageVariable _foodProductionPerSecond;
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
            _epochData = new EpochStorageData(_globalStorageProvider);
            _foodProductionPerSecond = new FloatStorageVariable("FoodProduction", _localStorageProvider, .25f);
            _curEpochIndex = new IntStorageVariable("CurrentEpoch", _localStorageProvider, 0);
            
            Debug.Log($"{this.GetType().Name} Initialized");
            return Task.CompletedTask;
        }


        public void Reset()
        {
            _localStorageProvider.Reset();
            _globalStorageProvider.Reset();
            
            _tutorialStorageData.Reset();
            _epochData.Reset();
            _purchasesStorageData.Reset();
        }

        public void AddMoney(uint Value)
        {
            _epochData.AddMoney(Value);
        }
        
        public void SetMoney(uint Value)
        {
            _epochData.Money = Value;
        }

        public void SetCurrentEpoch(int index)
        {
            _curEpochIndex.Value = index;
        }

        public void SetFoodProductionPerSecond(float value)
        {
            _foodProductionPerSecond.Value = value;
        }
        
        public uint UserMoney
        {
            get { return _epochData.Money; }
        }

        public EpochStorageData EpochData => _epochData;


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

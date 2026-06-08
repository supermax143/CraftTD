using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.Game;
using UnityEngine;

namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// JSON-serializable structure for user data storage.
    /// Contains money and weapon levels.
    /// </summary>
    [System.Serializable]
    internal class EpochDataInfo
    {
        public uint Money;
        public uint FoodProductionLevel;
        public uint TowerUpgradeLevel;
        public readonly List<UnitTier> _openedUnits = new();
    }

    internal class EpochStorageData
    {
        private const string EPOCH_DATA_KEY = "EpochData";
        
        private EpochDataInfo _epochDataInfo = new EpochDataInfo();
        private readonly StringStorageVariable _userDataVariable;
        
        private readonly IStorageProvider _storageProvider;

        public EpochStorageData(IStorageProvider storageProvider)
        {
            _userDataVariable = new StringStorageVariable(EPOCH_DATA_KEY, storageProvider);
            
            var userDataInfo = LoadUserDataInfo();
            if (userDataInfo != null)
            {
                _epochDataInfo = userDataInfo;
            }
            else
            {
                InitializeDefaultData();
            }
        }

        public uint Money
        {
            get => _epochDataInfo.Money;
            set
            {
                _epochDataInfo.Money = value;
                Save();
            }
        }

        

        public void AddMoney(uint amount)
        {
            _epochDataInfo.Money += amount;
            Save();
        }

        public void SetMoney(uint amount)
        {
            _epochDataInfo.Money = amount;
            Save();
        }

        public void Reset()
        {
            InitializeDefaultData();
            Save();
        }

        private void InitializeDefaultData()
        {
            _epochDataInfo = new EpochDataInfo
            {
                Money = 0,
            };
        }

        private void Save()
        {
            _userDataVariable.Value = SerializeToJson();
        }
        
        private string SerializeToJson()
        {
            return JsonConvert.SerializeObject(_epochDataInfo, Formatting.Indented);
        }
        
        private EpochDataInfo LoadUserDataInfo()
        {
            var json = _userDataVariable.Value;
            if (string.IsNullOrEmpty(json))
                return null;
                
            try
            {
                return JsonConvert.DeserializeObject<EpochDataInfo>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load user data: {e.Message}");
                return null;
            }
        }
        
    }
}

using System.Collections.Generic;
using Core.Application.Models;
using Newtonsoft.Json;
using Unity.Game;
using UnityEngine;

namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// JSON-serializable structure for user data storage.
    /// Contains weapon levels and opened units.
    /// </summary>
    [System.Serializable]
    internal class EpochDataInfo
    {
        public uint FoodProductionLevel = 0;
        public uint TowerLevel = 0;
        public List<UnitTier> OpenedUnits = new();
        public List<UnitTier> OpenedDefenseStances = new();
    }

    internal class EpochStorageData
    {
        private const string EPOCH_DATA_KEY = "EpochData";
        
        private EpochDataInfo _epochDataInfo = new EpochDataInfo();
        private readonly StringStorageVariable _epochDataVariable;
        
        private readonly IStorageProvider _storageProvider;

        public EpochStorageData(IStorageProvider storageProvider)
        {
            _epochDataVariable = new StringStorageVariable(EPOCH_DATA_KEY, storageProvider);
            
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

        
        public uint FoodProductionLevel
        {
            get => _epochDataInfo.FoodProductionLevel;
            set
            {
                _epochDataInfo.FoodProductionLevel = value;
                Save();
            }
        }
        
        
        public uint TowerLevel
        {
            get => _epochDataInfo.TowerLevel;
            set
            {
                _epochDataInfo.TowerLevel = value;
                Save();
            }
        }
        
        
        public bool IsUnitOpened(UnitTier tier)
        {
            return _epochDataInfo.OpenedUnits.Contains(tier);
        }
        
        public IEnumerable<UnitTier> GetOpenedUnits()
        {
            return _epochDataInfo.OpenedUnits;
        }
        
        public void OpenUnit(UnitTier tier)
        {
            if (_epochDataInfo.OpenedUnits.Contains(tier))
            {
                return;
            }
            _epochDataInfo.OpenedUnits.Add(tier);
            Save();
        }

        public bool IsDefenseStanceOpened(UnitTier tier)
        {
            return _epochDataInfo.OpenedDefenseStances.Contains(tier);
        }

        public void OpenDefenseStance(UnitTier tier)
        {
            if (_epochDataInfo.OpenedDefenseStances.Contains(tier))
            {
                return;
            }
            _epochDataInfo.OpenedDefenseStances.Add(tier);
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
                FoodProductionLevel = 0,
                TowerLevel = 0,
                OpenedUnits = new List<UnitTier>() { UnitTier.Tier1 }
            };
        }

        private void Save()
        {
            _epochDataVariable.Value = SerializeToJson();
        }
        
        private string SerializeToJson()
        {
            return JsonConvert.SerializeObject(_epochDataInfo, Formatting.Indented);
        }
        
        private EpochDataInfo LoadUserDataInfo()
        {
            var json = _epochDataVariable.Value;
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

using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// DTO for inventory item serialization (only id and type)
    /// </summary>
    [System.Serializable]
    public class InventoryItemData
    {
        public int Id;
        public InventoryItemType Type;
    }

    /// <summary>
    /// JSON-serializable structure for inventory data storage.
    /// Contains resources array and items.
    /// </summary>
    [System.Serializable]
    internal class InventoryDataInfo
    {
        public Resource[] Resources = new Resource[3];
        public List<InventoryItemData> Items = new();
    }

    public class InventoryStorageData
    {
        public event Action OnReset;
        private const string INVENTORY_DATA_KEY = "InventoryData";

        private InventoryDataInfo _inventoryDataInfo = new InventoryDataInfo();
        private readonly StringStorageVariable _inventoryDataVariable;

        private readonly IStorageProvider _storageProvider;

        public InventoryStorageData(IStorageProvider storageProvider)
        {
            _inventoryDataVariable = new StringStorageVariable(INVENTORY_DATA_KEY, storageProvider);

            var dataInfo = LoadDataInfo();
            if (dataInfo != null)
            {
                _inventoryDataInfo = dataInfo;
            }
            else
            {
                InitializeDefaultData();
            }
        }

        public Resource GetResource(ResourceType resourceType)
        {
            return _inventoryDataInfo.Resources[(int)resourceType];
        }

        public void SetResource(Resource resource)
        {
            _inventoryDataInfo.Resources[(int)resource.Type] = resource;
            Save();
        }

        public void WithdrawResource(Resource resource)
        {
            var res = _inventoryDataInfo.Resources[(int)resource.Type];
            if (res.Value < resource.Value)
            {
                throw new Exception("Not enough resources");
            }
            _inventoryDataInfo.Resources[(int)resource.Type].Value -= resource.Value;
            Save();
        }
        
        public void AddResource(Resource resource)
        {
            _inventoryDataInfo.Resources[(int)resource.Type].Value += resource.Value;
            Save();
        }
        
        public IReadOnlyList<InventoryItemData> GetItemsData()
        {
            return _inventoryDataInfo.Items.AsReadOnly();
        }

        public void AddItemData(InventoryItemData itemData)
        {
            _inventoryDataInfo.Items.Add(itemData);
            Save();
        }

        public void RemoveItem(int itemId)
        {
            _inventoryDataInfo.Items.RemoveAll(i => i.Id == itemId);
            Save();
        }

        public bool HasItem(int itemId)
        {
            return _inventoryDataInfo.Items.Any(i => i.Id == itemId);
        }

        public InventoryItemData GetItemData(int itemId)
        {
            return _inventoryDataInfo.Items.FirstOrDefault(i => i.Id == itemId);
        }

        public void ResetResource(ResourceType resourceType)
        {
            _inventoryDataInfo.Resources[(int)resourceType] = new Resource(resourceType, 0);
            Save();
        }

        public void Reset()
        {
            InitializeDefaultData();
            Save();
            OnReset?.Invoke();
        }

        public void ResetMoney()
        {
            SetResource(Resource.Money(0));
        }
        
        private void InitializeDefaultData()
        {
            _inventoryDataInfo = new InventoryDataInfo
            {
                Resources = new Resource[3]
                {
                    Resource.Food(0),
                    Resource.Money(0),
                    Resource.Crystal(0)
                },
                Items = new List<InventoryItemData>()
            };
        }

        private void Save()
        {
            _inventoryDataVariable.Value = SerializeToJson();
        }

        private string SerializeToJson()
        {
            return JsonConvert.SerializeObject(_inventoryDataInfo, Formatting.Indented);
        }

        private InventoryDataInfo LoadDataInfo()
        {
            var json = _inventoryDataVariable.Value;
            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<InventoryDataInfo>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load inventory data: {e.Message}");

                return null;
            }
        }

    }
}

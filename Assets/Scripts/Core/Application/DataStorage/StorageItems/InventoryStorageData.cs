using System.Collections.Generic;
using System.Linq;
using Core.Application.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// JSON-serializable structure for inventory data storage.
    /// Contains resources (Money, Crystal) and items.
    /// </summary>
    [System.Serializable]
    internal class InventoryDataInfo
    {
        public Resource Money = Resource.Money(0);
        public Resource Crystal = Resource.Crystal(0);
        public List<Item> Items = new();
    }

    public class InventoryStorageData
    {
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

        public Resource Money
        {
            get => _inventoryDataInfo.Money;
            set
            {
                _inventoryDataInfo.Money = value;
                Save();
            }
        }

        public Resource Crystal
        {
            get => _inventoryDataInfo.Crystal;
            set
            {
                _inventoryDataInfo.Crystal = value;
                Save();
            }
        }

        public IReadOnlyList<Item> GetItems()
        {
            return _inventoryDataInfo.Items.AsReadOnly();
        }

        public void AddItem(Item item)
        {
            _inventoryDataInfo.Items.Add(item);
            Save();
        }

        public void RemoveItem(string itemId)
        {
            _inventoryDataInfo.Items.RemoveAll(i => i.Id == itemId);
            Save();
        }

        public bool HasItem(string itemId)
        {
            return _inventoryDataInfo.Items.Any(i => i.Id == itemId);
        }

        public Item GetItem(string itemId)
        {
            return _inventoryDataInfo.Items.FirstOrDefault(i => i.Id == itemId);
        }

        public void ResetMoney()
        {
            _inventoryDataInfo.Money = Resource.Money(0);
            Save();
        }

        public void Reset()
        {
            InitializeDefaultData();
            Save();
        }

        private void InitializeDefaultData()
        {
            _inventoryDataInfo = new InventoryDataInfo
            {
                Money = Resource.Money(0),
                Crystal = Resource.Crystal(0),
                Items = new List<Item>()
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

using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.DataStorage;
using Core.Application.DataStorage.StorageItems;
using Core.Application.Info.Inventory;
using Zenject;

namespace Core.Application.Models
{
    public class InventoryModel : IInventoryModel
    {
        public event Action OnItemsChanged;
        public event Action<ResourceType> OnResourceChanged;
        
        [Inject] private readonly IDataStorage _dataStorage;
        [Inject] private readonly InventoryConfig _inventoryConfig;
        
        private InventoryStorageData InventoryData => _dataStorage.Inventory;

       
        public Resource Money
        {
            get => InventoryData.GetResource(ResourceType.Money);
            set
            {
                InventoryData.SetResource(value);
                OnResourceChanged?.Invoke(ResourceType.Money);
            }
        }

        public Resource Crystal
        {
            get => InventoryData.GetResource(ResourceType.Crystal);
            set
            {
                InventoryData.SetResource(value);
                OnResourceChanged?.Invoke(ResourceType.Crystal);
            }
        }

        
        public void WithdrawResource(Resource resource)
        {
            if (resource.Value > GetResourceCount(resource.Type))
            {
                return;
            }
            InventoryData.WithdrawResource(resource);
            OnResourceChanged?.Invoke(resource.Type);
        }
        
        public void WithdrawResource(ResourceType resourceType, int amount)
        {
            WithdrawResource(new Resource(resourceType, amount));
        }
        
        public void AddResource(ResourceType resourceType, int amount)
        {
            AddResource(new Resource(resourceType, amount));
        }
        
        public void AddResource(Resource resource)
        {
            InventoryData.AddResource(resource);
            OnResourceChanged?.Invoke(resource.Type);
        }
        
        public int GetResourceCount(ResourceType resourceType) 
            => InventoryData.GetResource(resourceType).Value;

        public bool HasEnough(ResourceType resourceType, int checkingValue) 
            => GetResourceCount(resourceType) >= checkingValue;
        
        public IReadOnlyList<InventoryItem> Items => InventoryData.GetItems();

        public void AddItem(InventoryItem inventoryItem)
        {
            InventoryData.AddItem(inventoryItem);
            OnItemsChanged?.Invoke();
        }

        public void RemoveItem(string itemId)
        {
            InventoryData.RemoveItem(itemId);
            OnItemsChanged?.Invoke();
        }

        public bool HasItem(string itemId)
        {
            return InventoryData.HasItem(itemId);
        }

        public InventoryItem GetItem(string itemId)
        {
            return InventoryData.GetItem(itemId);
        }
        
        public bool TryGetItemConfig(InventoryItemType itemType, out InventoryItemConfig itemConfig)
        {
            itemConfig = _inventoryConfig.Items.FirstOrDefault(i => i.Type == itemType);
            return itemConfig != null;
        }
    }
}

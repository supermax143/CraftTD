using System;
using System.Collections.Generic;
using Core.Application.DataStorage;
using Core.Application.DataStorage.StorageItems;
using Zenject;

namespace Core.Application.Models
{
    public class InventoryModel : IInventoryModel
    {
        public event Action OnMoneyChanged;
        public event Action OnCrystalChanged;
        public event Action OnItemsChanged;

        
        [Inject] private readonly IDataStorage _dataStorage;
        
        private InventoryStorageData InventoryData => _dataStorage.Inventory;

       

        public Resource Money
        {
            get => InventoryData.Money;
            set
            {
                InventoryData.Money = value;
                OnMoneyChanged?.Invoke();
            }
        }

        public Resource Crystal
        {
            get => InventoryData.Crystal;
            set
            {
                InventoryData.Crystal = value;
                OnCrystalChanged?.Invoke();
            }
        }

        public IReadOnlyList<Item> Items => InventoryData.GetItems();

        public void AddItem(Item item)
        {
            InventoryData.AddItem(item);
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

        public Item GetItem(string itemId)
        {
            return InventoryData.GetItem(itemId);
        }
    }
}

using System;
using System.Collections.Generic;
using Core.Application.DataStorage.StorageItems;

namespace Core.Application.Models
{
    public class InventoryModel
    {
        public event Action OnMoneyChanged;
        public event Action OnCrystalChanged;
        public event Action OnItemsChanged;

        private readonly InventoryStorageData _data;

        public InventoryModel(InventoryStorageData data)
        {
            _data = data;
        }

        public Resource Money
        {
            get => _data.Money;
            set
            {
                _data.Money = value;
                OnMoneyChanged?.Invoke();
            }
        }

        public Resource Crystal
        {
            get => _data.Crystal;
            set
            {
                _data.Crystal = value;
                OnCrystalChanged?.Invoke();
            }
        }

        public IReadOnlyList<Item> Items => _data.GetItems();

        public void AddItem(Item item)
        {
            _data.AddItem(item);
            OnItemsChanged?.Invoke();
        }

        public void RemoveItem(string itemId)
        {
            _data.RemoveItem(itemId);
            OnItemsChanged?.Invoke();
        }

        public bool HasItem(string itemId)
        {
            return _data.HasItem(itemId);
        }

        public Item GetItem(string itemId)
        {
            return _data.GetItem(itemId);
        }
    }
}

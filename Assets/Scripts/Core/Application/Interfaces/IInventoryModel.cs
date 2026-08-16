using System;
using Core.Application.Info.Inventory;

namespace Core.Application.Models
{
    public interface IInventoryModel
    {
        int GetResourceCount(ResourceType resourceType);
        event Action<ResourceType> OnResourceChanged;
        event Action OnItemsChanged;
        Resource Money { get; set; }
        Resource Crystal { get; set; }
        bool TryGetItemConfig(InventoryItemType itemType, out InventoryItemConfig itemConfig);
        void AddResource(ResourceType resourceType, int amount);
        void AddResource(Resource resource);
        void WithdrawResource(Resource resource);
        void WithdrawResource(ResourceType resourceType, int amount);
        bool HasEnough(ResourceType resourceType, int checkingValue);
        void AddItem(InventoryItemType itemType);
        void RemoveItem(int itemId);
    }
}
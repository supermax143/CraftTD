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
    }
}
using System;

namespace Core.Application.Models
{
    public interface IInventoryModel
    {
        int GetResourceCount(ResourceType resourceType);
        event Action<ResourceType> OnResourceChanged;
        event Action OnItemsChanged;
        Resource Money { get; set; }
        Resource Crystal { get; set; }
    }
}
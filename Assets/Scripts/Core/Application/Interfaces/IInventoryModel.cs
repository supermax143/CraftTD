namespace Core.Application.Models
{
    public interface IInventoryModel
    {
        int GetResourceCount(ResourceType resourceType);
    }
}
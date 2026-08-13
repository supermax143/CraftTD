using Core.Application.Models;
using UnityEngine;

namespace Core.Application.Info.Shop
{
    [System.Serializable]
    public struct Reward
    {
        [SerializeField] private RewardType _rewardType;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private InventoryItemType inventoryItemType;
        [SerializeField] private int _count;

        public RewardType RewardType => _rewardType;
        public ResourceType ResourceType => _resourceType;
        public int Count => _count;
        public InventoryItemType InventoryItemType => inventoryItemType;

        public Reward(RewardType rewardType, ResourceType resourceType, int count, InventoryItemType inventoryItemType = Models.InventoryItemType.None)
        {
            _rewardType = rewardType;
            _resourceType = resourceType;
            _count = count;
            this.inventoryItemType = inventoryItemType;
        }

        public static Reward ResourceReward(ResourceType resourceType, int count)
        {
            return new Reward(RewardType.Resource, resourceType, count);
        }

        public static Reward ItemReward(InventoryItemType inventoryItemType, int count)
        {
            return new Reward(RewardType.Item, ResourceType.Food, 0, inventoryItemType);
        }
    }
}

using Core.Application.Models;
using UnityEngine;

namespace Core.Application.Info.Shop
{
    [System.Serializable]
    public struct Reward
    {
        [SerializeField] private RewardType _rewardType;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private int _count;

        public RewardType RewardType => _rewardType;
        public ResourceType ResourceType => _resourceType;
        public int Count => _count;
        public ItemType ItemType => _itemType;

        public Reward(RewardType rewardType, ResourceType resourceType, int count, ItemType itemType = Models.ItemType.TestItem)
        {
            _rewardType = rewardType;
            _resourceType = resourceType;
            _count = count;
            _itemType = itemType;
        }

        public static Reward ResourceReward(ResourceType resourceType, int count)
        {
            return new Reward(RewardType.Resource, resourceType, count);
        }

        public static Reward ItemReward(ItemType itemType, int count)
        {
            return new Reward(RewardType.Item, ResourceType.Food, 0, itemType);
        }
    }
}

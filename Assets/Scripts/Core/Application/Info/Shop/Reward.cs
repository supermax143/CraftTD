using Core.Application.Models;
using UnityEngine;

namespace Core.Application.Info.Shop
{
    [System.Serializable]
    public struct Reward
    {
        [SerializeField] private RewardType _rewardType;
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private int _resourceAmount;
        [SerializeField] private string _itemId;

        public RewardType RewardType => _rewardType;
        public ResourceType ResourceType => _resourceType;
        public int ResourceAmount => _resourceAmount;
        public string ItemId => _itemId;

        public Reward(RewardType rewardType, ResourceType resourceType, int resourceAmount, string itemId = null)
        {
            _rewardType = rewardType;
            _resourceType = resourceType;
            _resourceAmount = resourceAmount;
            _itemId = itemId;
        }

        public static Reward ResourceReward(ResourceType resourceType, int amount)
        {
            return new Reward(RewardType.Resource, resourceType, amount);
        }

        public static Reward ItemReward(string itemId)
        {
            return new Reward(RewardType.Item, ResourceType.Food, 0, itemId);
        }
    }
}

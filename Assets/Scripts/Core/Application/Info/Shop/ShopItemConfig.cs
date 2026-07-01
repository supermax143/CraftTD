using Core.Application.Models;
using UnityEngine;

namespace Core.Application.Info.Shop
{
    /// <summary>
    /// Конфигурация айтема магазина с параметрами покупки и награды
    /// </summary>
    [CreateAssetMenu(menuName = "CraftTD/ShopItemConfig", order = 2)]
    public class ShopItemConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _displayName;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _icon;
        [SerializeField] private PaymentType _paymentType;

        [SerializeField] private ResourceType _currencyType;
        [SerializeField] private int _price;

        [SerializeField] private bool _isConsumable;
        [SerializeField] private string _localizedPricePlaceholder;

        [SerializeField] private RewardType _rewardType;
        [SerializeField] private ResourceType _rewardResourceType;
        [SerializeField] private int _rewardAmount;

        public string Id => _id;
        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
        public PaymentType PaymentType => _paymentType;
        public ResourceType CurrencyType => _currencyType;
        public int Price => _price;
        public bool IsConsumable => _isConsumable;
        public string LocalizedPricePlaceholder => _localizedPricePlaceholder;
        public RewardType RewardType => _rewardType;
        public ResourceType RewardResourceType => _rewardResourceType;
        public int RewardAmount => _rewardAmount;
    }
}

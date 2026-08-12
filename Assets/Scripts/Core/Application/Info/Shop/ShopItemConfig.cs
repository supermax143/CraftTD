using Core.Application.Models;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Application.Info.Shop
{
    /// <summary>
    /// Конфигурация айтема магазина с параметрами покупки и награды
    /// </summary>
    [CreateAssetMenu(menuName = "CraftTD/ShopItemConfig", order = 2)]
    public class ShopItemConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private AssetReference _prefab;
        [SerializeField] private PaymentType _paymentType;

        [SerializeField] private ResourceType _currencyType;
        [SerializeField] private int _price;

        [SerializeField] private bool _isConsumable;

        [SerializeField] private Reward[] _rewards;

        public string Id => _id;
        public string Name => _name;
        public string Description => _description;
        public AssetReference Prefab => _prefab;
        public PaymentType PaymentType => _paymentType;
        public ResourceType CurrencyType => _currencyType;
        public int Price => _price;
        public bool IsConsumable => _isConsumable;
        public Reward[] Rewards => _rewards;
    }
}

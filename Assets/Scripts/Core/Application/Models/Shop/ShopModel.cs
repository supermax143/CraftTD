using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.DataStorage;
using Core.Application.DataStorage.StorageItems;
using Core.Application.Info.Shop;
using Unity.Infrastructure.Purchases;
using Zenject;

namespace Core.Application.Models
{
    /// <summary>
    /// Модель магазина для управления покупками за игровую и реальную валюту
    /// </summary>
    public class ShopModel : IShopModel, IInitializable
    {
        [Inject] private readonly ShopConfig _config;
        [Inject] private readonly IInventoryModel _inventory;
        [Inject] private readonly IDataStorage _dataStorage;
        [Inject] private readonly IPurchasesController _purchases;

        
        
        private PurchasesStorageData Purchases => _dataStorage.Purchases;
        
        public IReadOnlyList<ShopItemConfig> Items => _config.Items;

        public void Initialize()
        {
            _purchases.OnPurchaseComplete += OnPurchaseComplete;
        }

        

        public ShopItemConfig GetItem(string itemId)
        {
            return _config.Items.FirstOrDefault(item => item.Id == itemId);
        }

        public bool IsPurchased(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null || item.PaymentType != PaymentType.RealMoney || item.IsConsumable)
            {
                return false;
            }
            return Purchases.GetPurchase(itemId) > 0;
        }

        public bool CanBuyWithGameCurrency(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null || item.PaymentType != PaymentType.GameCurrency)
            {
                return false;
            }

            return _inventory.HasEnough(item.CurrencyType, item.Price);
        }

        public bool BuyWithGameCurrency(string itemId)
        {
            if (!CanBuyWithGameCurrency(itemId))
            {
                return  false;
            }
            var item = GetItem(itemId);
            _inventory.WithdrawResource(item.CurrencyType, item.Price);
            GrantReward(item);
            return true;
        }

        public bool TryGetItemWithResourceForRealMoney(ResourceType resourceType, out ShopItemConfig shopItem)
        {
            shopItem = _config.Items.
                FirstOrDefault(item => 
                    item.PaymentType == PaymentType.RealMoney && 
                    item.CurrencyType == resourceType &&
                    item.IsConsumable); 
            return shopItem != null;
        }

        private void OnPurchaseComplete(ShopItemConfig item)
        {
            GrantReward(item);
        }
        
        private void GrantReward(ShopItemConfig item)
        {
            foreach (var reward in item.Rewards)
            {
                if (reward.RewardType == RewardType.Resource)
                {
                    _inventory.AddResource(reward.ResourceType, reward.Count);
                }
                else if (reward.RewardType == RewardType.Item)
                {
                    _inventory.AddItem(reward.ItemType);
                }
            }
        }

        
    }
}

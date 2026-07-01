using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.DataStorage;
using Core.Application.DataStorage.StorageItems;
using Core.Application.Info.Shop;
using Zenject;

namespace Core.Application.Models
{
    /// <summary>
    /// Модель магазина для управления покупками за игровую и реальную валюту
    /// </summary>
    public class ShopModel : IShopModel
    {
        public event Action<string> OnItemPurchased;

        [Inject] private readonly ShopConfig _config;
        [Inject] private readonly InventoryModel _inventory;
        [Inject] private readonly IDataStorage _dataStorage;
        
        
        
        private PurchasesStorageData Purchases => _dataStorage.Purchases;

        
        public IReadOnlyList<ShopItemConfig> Items => _config.Items;

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

        public bool CanBuyWithCurrency(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null || item.PaymentType != PaymentType.GameCurrency)
            {
                return false;
            }

            var currentBalance = item.CurrencyType == ResourceType.Money 
                ? _inventory.Money.Value 
                : _inventory.Crystal.Value;

            return currentBalance >= item.Price;
        }

        public bool BuyWithCurrency(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null || item.PaymentType != PaymentType.GameCurrency)
            {
                return false;
            }

            if (!CanBuyWithCurrency(itemId))
            {
                return false;
            }

            if (item.CurrencyType == ResourceType.Money)
            {
                _inventory.Money = new Resource(ResourceType.Money, _inventory.Money.Value - item.Price);
            }
            else if (item.CurrencyType == ResourceType.Crystal)
            {
                _inventory.Crystal = new Resource(ResourceType.Crystal, _inventory.Crystal.Value - item.Price);
            }

            GrantReward(item);
            OnItemPurchased?.Invoke(itemId);
            return true;
        }

        public void GrantRealMoneyPurchase(string itemId)
        {
            var item = GetItem(itemId);
            if (item == null)
            {
                return;
            }

            if (item.PaymentType == PaymentType.RealMoney && !item.IsConsumable)
            {
                if (IsPurchased(itemId))
                {
                    return;
                }
                Purchases.AddPurchase(itemId);
            }

            GrantReward(item);
            OnItemPurchased?.Invoke(itemId);
        }

        private void GrantReward(ShopItemConfig item)
        {
            if (item.RewardType == RewardType.Resource)
            {
                if (item.RewardResourceType == ResourceType.Money)
                {
                    _inventory.Money = new Resource(ResourceType.Money, _inventory.Money.Value + item.RewardAmount);
                }
                else if (item.RewardResourceType == ResourceType.Crystal)
                {
                    _inventory.Crystal = new Resource(ResourceType.Crystal, _inventory.Crystal.Value + item.RewardAmount);
                }
                else if (item.RewardResourceType == ResourceType.Food)
                {
                    // Food handling if needed in future
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Core.Application.Info.Shop;

namespace Core.Application.Models
{
    public interface IShopModel
    {
        event Action<string> OnItemPurchased;
        
        IReadOnlyList<ShopItemConfig> Items { get; }
        ShopItemConfig GetItem(string itemId);
        bool IsPurchased(string itemId);
        bool CanBuyWithGameCurrency(string itemId);
        bool BuyWithGameCurrency(string itemId);
        void GrantRealMoneyPurchase(string itemId);
    }
}

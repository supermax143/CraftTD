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
        bool CanBuyWithCurrency(string itemId);
        bool BuyWithCurrency(string itemId);
        void GrantRealMoneyPurchase(string itemId);
    }
}

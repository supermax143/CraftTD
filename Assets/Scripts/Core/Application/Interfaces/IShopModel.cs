using System;
using System.Collections.Generic;
using Core.Application.Info.Shop;

namespace Core.Application.Models
{
    public interface IShopModel
    {
        IReadOnlyList<ShopItemConfig> Items { get; }
        ShopItemConfig GetItem(string itemId);
        bool IsPurchased(string itemId);
        bool CanBuyWithGameCurrency(string itemId);
        bool BuyWithGameCurrency(string itemId);
        bool TryGetItemWithResourceForRealMoney(ResourceType resourceType, out ShopItemConfig shopItem);
    }
}

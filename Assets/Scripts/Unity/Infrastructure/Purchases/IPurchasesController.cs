using System;
using System.Threading.Tasks;
using Core.Application.Info.Shop;
using Core.Application.Interfaces;

namespace Unity.Infrastructure.Purchases
{
    public interface IPurchasesController : IBootstrapStep
    {
        event Action<ShopItemConfig> OnPurchaseComplete;
        void BuyProduct(string purchaseItemId);
    }
}
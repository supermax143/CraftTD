using System;
using System.Threading.Tasks;
using Core.Application.Interfaces;

namespace Unity.Infrastructure.Purchases
{
    public interface IPurchasesController : IBootstrapStep
    {
        event Action<string> OnPurchaseComplete;
        void BuyProduct(string purchaseItemId);
    }
}
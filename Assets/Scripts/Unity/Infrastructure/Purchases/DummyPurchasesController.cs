using System;
using System.Threading.Tasks;
using Core.Application.Interfaces;
using UnityEngine;

namespace Unity.Infrastructure.Purchases
{
    public class DummyPurchasesController : IPurchasesController
    {
        public event Action<string> OnPurchaseComplete;
        
        public Task Initialize()
        {
            Debug.Log($"{this.GetType().Name} Initialized");
            return Task.CompletedTask;
        }

        public void BuyProduct(string purchaseItemId)
        {
            Debug.Log($"{this.GetType().Name} purchase bought:{purchaseItemId}");
            OnPurchaseComplete?.Invoke(purchaseItemId);
        }
    }
}
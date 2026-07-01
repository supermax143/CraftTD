using System;
using System.Threading.Tasks;
using Core.Application.Interfaces;
using Core.Application.Models;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Purchases
{
    public class DummyPurchasesController : IPurchasesController
    {
        public event Action<string> OnPurchaseComplete;

        [Inject] private IShopModel _shopModel;

        public Task Init()
        {
            Debug.Log($"{this.GetType().Name} Initialized");
            return Task.CompletedTask;
        }

        public void BuyProduct(string purchaseItemId)
        {
            Debug.Log($"{this.GetType().Name} purchase bought:{purchaseItemId}");
            _shopModel.GrantRealMoneyPurchase(purchaseItemId);
            OnPurchaseComplete?.Invoke(purchaseItemId);
        }
    }
}
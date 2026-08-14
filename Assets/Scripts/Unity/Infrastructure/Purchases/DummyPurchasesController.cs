using System;
using System.Threading.Tasks;
using Core.Application.DataStorage;
using Core.Application.Info.Shop;
using Core.Application.Interfaces;
using Core.Application.Models;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Purchases
{
    public class DummyPurchasesController : IPurchasesController
    {
        public event Action<ShopItemConfig> OnPurchaseComplete;

        [Inject] private IShopModel _shopModel;
        [Inject] private IDataStorage _dataStorage;
        
        public Task Init()
        {
            Debug.Log($"{this.GetType().Name} Initialized");
            return Task.CompletedTask;
        }

        public void BuyProduct(string itemId)
        {
            var item = _shopModel.GetItem(itemId);
            if (!item.IsConsumable)
            {
                _dataStorage.Purchases.AddPurchase(itemId);
            }
            OnPurchaseComplete?.Invoke(item);
        }
    }
}
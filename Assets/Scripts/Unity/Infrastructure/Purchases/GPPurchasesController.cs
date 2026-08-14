using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.DataStorage;
using Core.Application.Info.Shop;
using Core.Application.Models;
using GamePush;
using GamePush.Initialization;
using Unity.Infrastructure.Purchases;
using UnityEngine;
using Zenject;

namespace Zombies.Purchases
{
    public class GPPurchasesController : IPurchasesController
    {

        public event Action<ShopItemConfig> OnPurchaseComplete;

        [Inject] private IShopModel _shopModel;
        [Inject] private IDataStorage _dataStorage;
        
        public bool ProductsInitialized { get; private set; }
        public bool PurchasesInitialized { get; private set; }
        
        public bool Initialized => ProductsInitialized && PurchasesInitialized;
        
        private TaskCompletionSource<bool> _initTask;
        
        private List<FetchPlayerPurchases> _purchases;
        private List<FetchProducts> _products;
        
        
#if DEBUG_MODE
        public async void Initialize()
        {
            await Init();
        }
#endif
        public async Task Init()
        {
            _initTask = new TaskCompletionSource<bool>();
            GP_Payments.OnFetchProducts += OnProductsFetched;
            GP_Payments.OnFetchPlayerPurchases += OnPlayerPurchasesFetched;
            GP_Payments.OnPurchaseSuccess += PurchaseCompleteHandler;
            GP_Payments.OnPurchaseError += PurchaseErrorHandler;
            
            GP_Payments.Fetch();
            await _initTask.Task;
            Debug.Log($"{this.GetType().Name} Initialized");
        }
        

        public void BuyProduct(string purchaseItemId)
        {
            try
            {
                GP_Payments.Purchase(purchaseItemId);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
        }


        private void OnPlayerPurchasesFetched(List<FetchPlayerPurchases> items)
        {
            _purchases = items;
            Debug.Log("purchases:\n" + string.Join("\n", _purchases.Select(p => $" tag: {p.tag}, id: {p.productId}")));
            foreach (var purchase in _purchases)
            {
                PurchaseCompleteHandler(purchase.tag);
            }
            
            PurchasesInitialized = true;
            CheckInitialized();
        }

        private void OnProductsFetched(List<FetchProducts> items)
        {
            _products = items;
            Debug.Log("products:\n" + string.Join("\n", _products.Select(p => $" id: {p.id}, name: {p.name}, price:{p.price}")));
            foreach (var item in _products)
            {
                /*_shopData.SetPurchaseId(item.tag, item.id);
                _shopData.SetPurchasePrice(item.tag, $"{item.price} {item.currency}");*/
            }
            
            ProductsInitialized = true;
            CheckInitialized();
        }

        
        private void PurchaseErrorHandler()
        {
            Debug.Log($"PurchaseErrorHandler: UnityEvent");
        }

        private void PurchaseCompleteHandler(string id)
        {
            Debug.Log($"PurchaseComplete: {id} ");
            var item = _shopModel.GetItem(id);
            if (!item.IsConsumable)
            {
                _dataStorage.Purchases.AddPurchase(id);
                OnPurchaseComplete?.Invoke(item);
                return;
            }
            
            GP_Payments.Consume(id, (id) =>
            {
                OnPurchaseComplete?.Invoke(item);
            });

        }
        
        private void CheckInitialized()
        {
            if (!Initialized)
            {
                return;
            }
            _initTask.TrySetResult(true);
        }

        
    }
}
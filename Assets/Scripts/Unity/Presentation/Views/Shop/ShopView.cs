using System;
using Core.Application.Info.Shop;
using Core.Application.Interfaces;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Infrastructure.Purchases;
using Unity.Infrastructure.ResourceManager;
using Unity.Infrastructure.Windows;
using Unity.Presentation.Components;
using Unity.Presentation.Views;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.Windows
{
    /// <summary>
    /// Окно магазина для покупки айтемов за игровую и реальную валюту
    /// </summary>
    public class ShopView : ViewBase
    {
        [SerializeField] private Transform _packsContainer;
        [SerializeField] private Transform _itemsContainer;
        
        [Inject] private IMainModel _model;
        [Inject] private IPurchasesController _purchasesController;
        [Inject] private DiContainer _container;
        [Inject] private IInventoryModel _inventory;
        [Inject] private IShopModel _shop;
        [Inject] private IResourceManager _resourceManager;
        
        public override void Initialize()
        {
            _inventory.OnResourceChanged += OnResourceChanged;
            BuildItems().Forget();
        }

        private async UniTask BuildItems()
        {
            foreach (var config in _shop.Items)
            {
                var parent = config.Rewards.Length > 1 ? _packsContainer : _itemsContainer;
                var prefab = await config.Prefab.LoadAssetReference<GameObject>(gameObject);
                var view = _container.InstantiatePrefabForComponent<ShopItemView>(prefab, parent);
                view.OnBuyClicked += OnBuyClicked;
                view.Initialize(config);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_itemsContainer.parent as RectTransform);
        }

        private void OnBuyClicked(ShopItemConfig config)
        {
            switch (config.PaymentType)
            {
                case PaymentType.GameCurrency:
                    _shop.BuyWithCurrency(config.Id);
                    break;
                case PaymentType.RealMoney:
                    _purchasesController.BuyProduct(config.Id);
                    break;
                case PaymentType.WatchingAds:
                    Debug.Log("ShowAds");
                    break;
            }
        }

        private void OnItemPurchased(string itemId)
        {
           
        }

        private void OnResourceChanged(ResourceType resourceType)
        {
        }

        private void OnDestroy()
        {
            _inventory.OnResourceChanged -= OnResourceChanged;
            _shop.OnItemPurchased -= OnItemPurchased;
        }
    }
}

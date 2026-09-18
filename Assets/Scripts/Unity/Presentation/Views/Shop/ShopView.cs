using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.Info.Shop;
using Core.Application.Interfaces;
using Core.Application.Interfaces.Windows;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Game;
using Unity.Infrastructure.Purchases;
using Unity.Infrastructure.ResourceManager;
using Unity.Infrastructure.Windows;
using Unity.Presentation.Components;
using Unity.Presentation.Views;
using Unity.Presentation.Windows.Alert;
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

        private readonly List<ShopItemView> _shopItemViews = new();

        [Inject] private IMainModel _model;
        [Inject] private IPurchasesController _purchasesController;
        [Inject] private DiContainer _container;
        [Inject] private IInventoryModel _inventory;
        [Inject] private IShopModel _shop;
        [Inject] private IResourceManager _resourceManager;
        [Inject] private IWindowsController _windows;
        [Inject] private DropManager _dropManager;
        
        public override void Initialize()
        {
            _purchasesController.OnPurchaseComplete += OnPurchaseComplete;
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
                _shopItemViews.Add(view);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(_itemsContainer.parent as RectTransform);
        }

        private void OnBuyClicked(ShopItemConfig config)
        {
            switch (config.PaymentType)
            {
                case PaymentType.GameCurrency:
                    BuyWithGameCurency(config);
                    break;
                case PaymentType.RealMoney:
                    _purchasesController.BuyProduct(config.Id);
                    break;
                case PaymentType.WatchingAds:
                    Debug.Log("ShowAds");
                    break;
            }
        }

        private void BuyWithGameCurency(ShopItemConfig item)
        {
            if (!_shop.CanBuyWithGameCurrency(item.Id))
            {
                _windows.ShowWindow<AlertWindow>((window) =>
                {
                    window.Setup(AlertWindowState.YesNo ,"!Not enough currency", "!You don't have enough currency. Buy some?");
                    window.Show();
                    window.OnResultSelected += result =>
                    {
                        if (result != AlertResult.Yes || 
                            !_shop.TryGetItemWithResourceForRealMoney(item.CurrencyType, out var shopItem))
                        {
                            return;
                        }
                        _purchasesController.BuyProduct(shopItem.Id);
                    };
                });
                return;
            }

            _shop.BuyWithGameCurrency(item.Id);
            ShowDrop(item);
        }


        public void ShowDrop(ShopItemConfig item)
        {
            var itemView = GetShopItemView(item.Id);
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
                null,
                itemView.transform.position
            );
            var pos = Camera.main.ScreenToWorldPoint(screenPoint);
            _dropManager.ShowUiDrop(item.Rewards.First().ToResource(), pos);
        }
        
        private void OnPurchaseComplete(ShopItemConfig item)
        {
            ShowDrop(item);
        }

        private void OnResourceChanged(ResourceType resourceType)
        {
        }

        public ShopItemView GetShopItemView(string itemId)
        {
            return _shopItemViews.Find(view => view.Config.Id == itemId);
        }

        private void OnDestroy()
        {
            _inventory.OnResourceChanged -= OnResourceChanged;
            foreach (var shopItem in _shopItemViews)
            {
                shopItem.OnBuyClicked -= OnBuyClicked;
            }
        }
    }
}

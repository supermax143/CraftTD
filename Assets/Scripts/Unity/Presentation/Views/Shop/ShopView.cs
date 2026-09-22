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
using Unity.Infrastructure.VisualActions;
using Unity.Infrastructure.VisualActions.ActionsData;
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
        //[Inject] private DropManager _dropManager;
        [Inject] private IActionsDispatcher _actionsDispatcher;
        
        public override void Initialize()
        {
            //_purchasesController.OnPurchaseComplete += OnPurchaseComplete;
            _shop.BeforeRewardGranted += OnBeforeRewardGranted;
            // _inventory.OnResourceChanged += OnResourceChanged;
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
                    window.Setup(AlertWindowState.YesNo ,"alert_not_enougth_crystals_label", 
                        "alert_not_enougth_crystals_description");
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

            // ShowDrop(item);
            _shop.BuyWithGameCurrency(item.Id);
        }

        private void OnBeforeRewardGranted(ShopItemConfig item)
        {
            ShowDrop(item);
        }

       
        
        /*private void OnPurchaseComplete(ShopItemConfig item)
        {
            ShowDrop(item);
        }*/

        public void ShowDrop(ShopItemConfig item)
        {
            var itemView = GetShopItemView(item.Id);
            _actionsDispatcher.AddAction(new ShowResourceDropActionData()
            {
                Resource = item.Rewards.First().ToResource(),
                StartPosition = itemView.transform.position,
                IsUiDrop = true
            });
        }
        
        public ShopItemView GetShopItemView(string itemId)
        {
            return _shopItemViews.Find(view => view.Config.Id == itemId);
        }

        private void OnDestroy()
        {
            // _inventory.OnResourceChanged -= OnResourceChanged;
            _shop.BeforeRewardGranted -= OnBeforeRewardGranted;
            foreach (var shopItem in _shopItemViews)
            {
                shopItem.OnBuyClicked -= OnBuyClicked;
            }
        }
    }
}

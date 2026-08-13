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
using Zenject;

namespace Unity.Presentation.Windows
{
    /// <summary>
    /// Окно магазина для покупки айтемов за игровую и реальную валюту
    /// </summary>
    public class ShopView : ViewBase
    {
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
                var prefab = await config.Prefab.LoadAssetReference<GameObject>(gameObject);
                var view = _container.InstantiatePrefabForComponent<ShopItemView>(prefab, _itemsContainer);
                view.Initialize(config);
            }
        }

        private void OnBuyClicked(ShopItemConfig config)
        {
            if (config.PaymentType == PaymentType.GameCurrency)
            {
                _shop.BuyWithCurrency(config.Id);
            }
            else
            {
                _purchasesController.BuyProduct(config.Id);
            }
        }

        private void OnItemPurchased(string itemId)
        {
            /*foreach (Transform child in _itemsContainer)
            {
                if (child.TryGetComponent<ShopItemView>(out var view))
                {
                    view.Refresh();
                }
            }*/
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

using System;
using Core.Application.Info.Shop;
using Core.Application.Models;
using TMPro;
using Unity.Infrastructure.Purchases;
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
        [SerializeField] private ShopItemView _itemViewPrefab;
        [SerializeField] private TextMeshProUGUI _moneyTF;
        [SerializeField] private TextMeshProUGUI _crystalTF;

        [Inject] private IMainModel _model;
        [Inject] private IPurchasesController _purchasesController;
        [Inject] private DiContainer _container;
        [Inject] private IInventoryModel _inventory;
        [Inject] private IShopModel _shop;
        
        public override void Initialize()
        {
            _inventory.OnResourceChanged += OnResourceChanged;
            //BuildItems();
        }

        private void BuildItems()
        {
            foreach (var config in _shop.Items)
            {
                var view = _container.InstantiatePrefabForComponent<ShopItemView>(_itemViewPrefab, _itemsContainer);
                view.Setup(config, _shop, OnBuyClicked);
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
            foreach (Transform child in _itemsContainer)
            {
                if (child.TryGetComponent<ShopItemView>(out var view))
                {
                    view.Refresh();
                }
            }
        }

        private void OnResourceChanged(ResourceType resourceType)
        {
            //TODO: реализовать
        }

        private void OnDestroy()
        {
            _inventory.OnResourceChanged -= OnResourceChanged;
            _shop.OnItemPurchased -= OnItemPurchased;
        }
    }
}

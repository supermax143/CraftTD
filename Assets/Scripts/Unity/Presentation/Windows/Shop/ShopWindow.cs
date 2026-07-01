using System;
using Core.Application.Info.Shop;
using Core.Application.Models;
using TMPro;
using Unity.Infrastructure.Purchases;
using Unity.Infrastructure.Windows;
using Unity.Presentation.Components;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows
{
    /// <summary>
    /// Окно магазина для покупки айтемов за игровую и реальную валюту
    /// </summary>
    [Window(nameof(ShopWindow))]
    public class ShopWindow : WindowBase
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private ShopItemView _itemViewPrefab;
        [SerializeField] private TextMeshProUGUI _moneyTF;
        [SerializeField] private TextMeshProUGUI _crystalTF;

        [Inject] private IMainModel _model;
        [Inject] private IPurchasesController _purchasesController;
        [Inject] private DiContainer _container;

        public override void Initialize()
        {
            _model.Inventory.OnMoneyChanged += UpdateCurrency;
            _model.Inventory.OnCrystalChanged += UpdateCurrency;
            _model.Shop.OnItemPurchased += OnItemPurchased;
            BuildItems();
            UpdateCurrency();
        }

        private void BuildItems()
        {
            foreach (var config in _model.Shop.Items)
            {
                var view = _container.InstantiatePrefabForComponent<ShopItemView>(_itemViewPrefab, _itemsContainer);
                view.Setup(config, _model.Shop, OnBuyClicked);
            }
        }

        private void OnBuyClicked(ShopItemConfig config)
        {
            if (config.PaymentType == PaymentType.GameCurrency)
            {
                _model.Shop.BuyWithCurrency(config.Id);
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

        private void UpdateCurrency()
        {
            _moneyTF.text = _model.Inventory.Money.Value.ToString();
            _crystalTF.text = _model.Inventory.Crystal.Value.ToString();
        }

        private void OnDestroy()
        {
            _model.Inventory.OnMoneyChanged -= UpdateCurrency;
            _model.Inventory.OnCrystalChanged -= UpdateCurrency;
            _model.Shop.OnItemPurchased -= OnItemPurchased;
        }
    }
}

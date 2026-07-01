using System;
using Core.Application.Info.Shop;
using Core.Application.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Presentation.Components
{
    /// <summary>
    /// View компонент для отображения айтема магазина
    /// </summary>
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameTF;
        [SerializeField] private TextMeshProUGUI _priceTF;
        [SerializeField] private Image _currencyIcon;
        [SerializeField] private Button _buyButton;
        [SerializeField] private GameObject _purchasedOverlay;

        private ShopItemConfig _config;
        private IShopModel _shopModel;
        private Action<ShopItemConfig> _onBuy;

        public void Setup(ShopItemConfig config, IShopModel shopModel, Action<ShopItemConfig> onBuy)
        {
            _config = config;
            _shopModel = shopModel;
            _onBuy = onBuy;

            _icon.sprite = config.Icon;
            _nameTF.text = config.DisplayName;

            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(() => _onBuy?.Invoke(_config));

            Refresh();
        }

        public void Refresh()
        {
            bool purchased = _config.PaymentType == PaymentType.RealMoney
                && !_config.IsConsumable
                && _shopModel.IsPurchased(_config.Id);

            _purchasedOverlay.SetActive(purchased);
            _buyButton.gameObject.SetActive(!purchased);

            if (_config.PaymentType == PaymentType.GameCurrency)
            {
                _priceTF.text = _config.Price.ToString();
                _buyButton.interactable = _shopModel.CanBuyWithCurrency(_config.Id);
            }
            else
            {
                _priceTF.text = "...";
                _buyButton.interactable = true;
            }
        }
    }
}

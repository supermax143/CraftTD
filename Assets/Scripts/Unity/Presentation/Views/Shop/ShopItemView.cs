using System;
using System.Threading.Tasks;
using Core.Application.Info.Shop;
using Core.Application.Interfaces;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Infrastructure.ResourceManager;
using Unity.Presentation.HUD;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.Components
{
    /// <summary>
    /// View компонент для отображения айтема магазина
    /// </summary>
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameTF;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _descriptionTF;
        [SerializeField] private ResourceButton _resourceButton;
        [SerializeField] private Button _realMoneyButton;
        [SerializeField] private Button _actionButton;
        [SerializeField] private InventoryItemContainer _itemContainer;
        
        [Inject] private ILocalization _localization;
        [Inject] private IResourceManager _resourceManager;
        [Inject] private IInventoryModel _inventory;
        
        private ShopItemConfig _config;

        public void Initialize(ShopItemConfig config)
        {
            _config = config;
            UpdateView();
        }

        private void UpdateView()
        {
            if (_nameTF != null)
            {
                _nameTF.text = _localization.Get(_config.Name);
            }

            if (_descriptionTF != null)
            {
                _descriptionTF.text = _localization.Get(_config.Description);
            }

            UpdateIcon().Forget();

            if (_inventory.TryGetItemConfig(InventoryItemType.DoubleReward, out var itemConfig))
            {
               _itemContainer.SetItemConfig(itemConfig); 
            }
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            _resourceButton?.gameObject.SetActive(false);
            _realMoneyButton?.gameObject.SetActive(false);
            _actionButton?.gameObject.SetActive(false);

            switch (_config.PaymentType)
            {
                case PaymentType.GameCurrency:
                    _resourceButton.gameObject.SetActive(true);
                    _resourceButton.SetPrice(new Resource(_config.CurrencyType, _config.Price));
                    break;
                case PaymentType.RealMoney:
                    _realMoneyButton.gameObject.SetActive(true);
                    _realMoneyButton.GetComponentInChildren<TMP_Text>().text = _config.Price.ToString();
                    break;
                case PaymentType.WatchingAds:
                    _actionButton.gameObject.SetActive(true);
                    break;
            }
        }

        private async UniTask UpdateIcon()
        {
            if (!_config.TryGetIcon(out var iconSource))
            {
                _icon?.gameObject.SetActive(false);
                return;
            }
            var icon = await iconSource.LoadAssetReference<Sprite>(iconSource.AssetGUID);
            _icon.sprite = icon;
        }
        
    }
}

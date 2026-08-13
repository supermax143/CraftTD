using System.Threading.Tasks;
using Core.Application.Info.Inventory;
using Core.Application.Interfaces;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Infrastructure.ResourceManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.HUD
{
    /// <summary>
    /// Контейнер для отображения айтема инвентаря с иконкой и текстом
    /// </summary>
    public class InventoryItemContainer : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TMP_Text _text;
        
        [Inject] private ILocalization _localization;
        
        private InventoryItemConfig _config;

        public InventoryItemConfig Config => _config;


        public async UniTask SetItem(InventoryItem item)
        {
            await SetItemConfig(item.Config);
        }
        
        public async UniTask SetItemConfig(InventoryItemConfig config)
        {
            _config = config;
            await UpdateIcon();
            UpdateLabel();
        }

        private void UpdateLabel()
        {
            _text.text = _localization.Get(_config.Label);
        }

        private async UniTask UpdateIcon()
        {
            if (_config == null || !_config.TryGetIcon(out var iconRef))
            {
                Debug.LogError($"{GetType().Name} has no icon for {_config?.Type}");
                return;
            }
            var icon = await iconRef.LoadAssetReference<Sprite>(iconRef.AssetGUID);
            _icon.sprite = icon;
        }

        public RectTransform GetTargetRect()
        {
            return _icon.rectTransform;
        }
    }
}

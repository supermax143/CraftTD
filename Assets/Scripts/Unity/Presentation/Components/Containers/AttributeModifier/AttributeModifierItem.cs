using System.Threading.Tasks;
using Core.Application.Info.Attributes.AttributeModifiers;
using Core.Application.Interfaces;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Game.Attributes;
using Unity.Infrastructure.ResourceManager;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.Components.Containers
{
    /// <summary>
    /// Контейнер для отображения модификатора атрибута с иконкой, названием и значением
    /// </summary>
    public class AttributeModifierItem : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TMP_Text _label;
        [SerializeField]
        private TMP_Text _value;

        [Inject] private IResourceManager _resourceManager;
        [Inject] private ILocalization _localization;

        private AttributeModifierBase _modifier;

        public async UniTask SetModifier(AttributeModifierBase modifier)
        {
            _modifier = modifier;
            await UpdateIcon();
            UpdateLabel();
            UpdateValue();
        }

        private void UpdateLabel()
        {
            if (_label != null)
            {
                string locale = _localization.GetAttributeLocale(_modifier.AttributeKind);
                _label.text = string.IsNullOrEmpty(locale) ? _modifier.AttributeKind.ToString() : locale;
            }
        }

        private void UpdateValue()
        {
            if (_value != null)
            {
                string prefix = _modifier.ModifierKind switch
                {
                    ModifierAttributeKind.Add => "+",
                    ModifierAttributeKind.Multiply => "x",
                    ModifierAttributeKind.Override => "",
                    _ => ""
                };
                _value.text = $"{prefix} {_modifier.Value}";
            }
        }

        private async UniTask UpdateIcon()
        {
            if (_modifier == null || !_resourceManager.TryGetAttributeIcon(_modifier.AttributeKind, out var iconRef))
            {
                Debug.LogError($"{GetType().Name} has no icon for {_modifier?.AttributeKind}");
                return;
            }
            var icon = await iconRef.LoadAssetReference<Sprite>(iconRef.AssetGUID);
            _icon.sprite = icon;
        }
    }
}

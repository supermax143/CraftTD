using System;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Application.Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.Components
{
    /// <summary>
    /// View компонент для отображения апгрейда спела
    /// </summary>
    public class SpellUpgradeItem : MonoBehaviour
    {
        public event Action<SpellModel> OnUpgradeClicked;
        
        [SerializeField] private TextMeshProUGUI _levelTF;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _descriptionTF;
        [SerializeField] private ResourceButton _resourceButton;
        [SerializeField] private Button _upgradeButton;
        
        [Inject] private ILocalization _localization;
        [Inject] private IInventoryModel _inventory;
        
        private SpellModel _spellModel;

        public void Initialize(SpellModel spellModel)
        {
            _spellModel = spellModel;
            UpdateView();
        }

        private void UpdateView()
        {
            if (_levelTF != null)
            {
                _levelTF.text = $"Level {_spellModel.CurrentLevel + 1}/{_spellModel.Config.MaxLevel + 1}";
            }

            if (_descriptionTF != null)
            {
                _descriptionTF.text = _localization.Get(_spellModel.Config.Description);
            }

            UpdateIcon();
            UpdateUpgradeButton();
        }

        private void UpdateIcon()
        {
            if (_icon != null && _spellModel.Config.Icon != null)
            {
                _icon.sprite = _spellModel.Config.Icon;
                _icon.gameObject.SetActive(true);
            }
            else
            {
                _icon?.gameObject.SetActive(false);
            }
        }

        private void UpdateUpgradeButton()
        {
            if (!_spellModel.TryGetNextUpgrade(out var upgrade))
            {
                _resourceButton?.gameObject.SetActive(false);
                _upgradeButton?.gameObject.SetActive(false);
                return;
            }

            _resourceButton?.gameObject.SetActive(true);
            _upgradeButton?.gameObject.SetActive(true);
            _resourceButton.SetPrice(upgrade.Cost);

            var canAfford = _inventory.HasEnough(upgrade.Cost.Type, upgrade.Cost.Value);
            _upgradeButton.interactable = canAfford;
        }

        public void Upgrade()
        {
            OnUpgradeClicked?.Invoke(_spellModel);
        }
    }
}

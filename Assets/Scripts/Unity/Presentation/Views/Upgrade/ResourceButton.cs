using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Application.Interfaces;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Shared.Utils;
using TMPro;
using Unity.Infrastructure.ResourceManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.Components
{
    public class ResourceButton : PointerDownClicker
    {
        
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
        [SerializeField] private bool _showNotEnough = true;
        [SerializeField] private bool _interactableWhenNotEnough = true;
        
        
        [Inject] private IResourceManager _resourceManager;
        [Inject] private IMainModel _model;
        [Inject] private IInventoryModel _inventory;
        
        
        private Resource _price;
        
        protected override bool Active => _inventory.GetResourceCount(_price.Type) >= _price.Value;
        
        public Button Button => _button;

        public void SetPrice(Resource price)
        {
            _price = price;
            UpdateView();
        }

        private void UpdateView()
        {
            _priceText.text = LargeNumberFormatter.Format(_price.Value);
            UpdateActive();
            UpdateIcon();
        }

        public void UpdateActive()
        {
            if (_showNotEnough)
            {
                var color = Active ? Color.white : Color.red;
                _priceText.color = color;
            }

            if (!_interactableWhenNotEnough)
            {
                _button.interactable = Active;
            }
        }

        private async UniTask UpdateIcon()
        {
            if (!_resourceManager.TryGetResourceIcon(_price.Type, out var iconRef))
            {
                Debug.LogError($"{GetType().Name} has no icon for {_price.Type}");
                return;
            }
            var icon = await iconRef.LoadAssetReference<Sprite>(iconRef.AssetGUID);
            _icon.sprite = icon;
        }
        


    }
}
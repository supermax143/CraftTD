using System;
using System.Threading.Tasks;
using Core.Application.Interfaces;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Shared.Utils;
using TMPro;
using Unity.Infrastructure.ResourceManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.HUD
{
    public class ResourceContainer : MonoBehaviour, IDropTarget
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private TMP_Text _text;
        [SerializeField] 
        private ResourceType _resourceType;
        
        
        [Inject] private IResourceManager _resourceManager;
        
        private Resource _resource;
        public ResourceType ResourceType => _resourceType;


        public void Awake()
        {
            if (_resource != default)
            {
                return;
            }
            _resource = new Resource(_resourceType, _resource.Value);
            UpdateCount();
            UpdateIcon();
        }

        
        private async UniTask UpdateIcon()
        {
            if (!_resourceManager.TryGetResourceIcon(_resource.Type, out var iconRef))
            {
                Debug.LogError($"{GetType().Name} has no icon for {_resourceType}");
                return;
            }
            var icon = await iconRef.LoadAssetReference<Sprite>(iconRef.AssetGUID);
            _icon.sprite = icon;
        }
        
        public void SetResource(Resource resource)
        {
            bool resourceChanged = _resource.Type != resource.Type;
            _resource = resource;
            if (resourceChanged)
            {
                UpdateIcon().Forget();
            }
            UpdateCount();
        }
        
        public void SetValue(int value)
        {
            _resource.Value = value;
            UpdateCount();
        }
        
        private void UpdateCount()
        {
            _text.text = LargeNumberFormatter.Format(_resource.Value);
        }

        public RectTransform GetTargetRect()
        {
            return _icon.rectTransform;
        }
        
        public void AddResource(Resource resource)
        {
            _resource += resource;
            UpdateCount();
        }
        
    }
}
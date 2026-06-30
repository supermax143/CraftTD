using System;
using System.Threading.Tasks;
using Core.Application.Interfaces;
using Core.Application.Models;
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

        public ResourceType Type => _resourceType;

        public void Awake()
        {
            _resource = new Resource(Type, _resource.Value);
            UpdateCount();
            UpdateIcon();
        }

        
        private async Task UpdateIcon()
        {
            if (!_resourceManager.TryGetResourceIcon(_resourceType, out var iconRef))
            {
                Debug.LogError($"{GetType().Name} has no icon for {_resourceType}");
                return;
            }
            var icon = await iconRef.LoadAssetReference<Sprite>(iconRef.AssetGUID);
            _icon.sprite = icon;
        }
        
        
        public void SetValue(int value)
        {
            _resource.Value = value;
            UpdateCount();
        }
        
        private void UpdateCount()
        {
            _text.text = _resource.Value.ToString();
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
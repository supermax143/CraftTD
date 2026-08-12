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
        private ResourceType resourceResourceType;
        
        
        [Inject] private IResourceManager _resourceManager;
        
        private Resource _resource;

        public ResourceType ResourceType => resourceResourceType;

        public void Awake()
        {
            _resource = new Resource(ResourceType, _resource.Value);
            UpdateCount();
            UpdateIcon();
        }

        
        private async Task UpdateIcon()
        {
            if (!_resourceManager.TryGetResourceIcon(resourceResourceType, out var iconRef))
            {
                Debug.LogError($"{GetType().Name} has no icon for {resourceResourceType}");
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
using System;
using Core.Application.Models;
using Shared.Utils;
using TMPro;
using UnityEngine;
using Zenject;
using Zombies;

namespace Unity.Presentation.HUD
{
    /// <summary>
    /// Контейнер для отображения ресурса с иконкой и счётчиком
    /// </summary>
    public class ResourceContainer : MonoBehaviour, IDropTarget
    {
        [SerializeField]
        private ResourceImage _icon;
        [SerializeField]
        private AnimatedCounter _text;
        [SerializeField] 
        private ResourceType _resourceType;
        
        private Resource _resource;
        public ResourceType ResourceType => _resourceType;


        public void Awake()
        {
            if (_resource != default)
            {
                return;
            }
            _resource = new Resource(_resourceType, _resource.Value);
            _icon.SetResourceType(_resourceType);
        }
        
        public void SetResource(Resource resource)
        {
            bool resourceChanged = _resource.Type != resource.Type;
            _resource = resource;
            if (resourceChanged)
            {
                _icon.SetResourceType(_resource.Type);
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
            _text.SetValue(_resource.Value);
        }

        public RectTransform GetTargetRect()
        {
            return _icon.GetRect();
        }
        
        public void AddResource(Resource resource)
        {
            _resource += resource;
            UpdateCount();
        }

        public void Clear()
        {
            _resource.Value = 0;
            UpdateCount();
        }
    }
}
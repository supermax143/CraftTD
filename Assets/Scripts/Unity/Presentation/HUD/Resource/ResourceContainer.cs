using Core.Application.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        
        private Resource _resource;

        public ResourceType Type => _resourceType;

        public void Awake()
        {
            _resource = new Resource(Type, 0);
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
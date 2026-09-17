using System.Threading;
using Core.Application.Interfaces;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Unity.Infrastructure.ResourceManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.HUD
{
    /// <summary>
    /// Компонент для отображения иконки ресурса с асинхронной загрузкой
    /// </summary>
    public class ResourceIcon : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;
        
        [Inject] private IResourceManager _resourceManager;
        
        private CancellationTokenSource _iconUpdateCts;
        private ResourceType _resourceType;

        public void SetResourceType(ResourceType resourceType)
        {
            if (_resourceType == resourceType)
            {
                return;
            }
            _resourceType = resourceType;
            UpdateIcon().Forget();
        }

        private async UniTask UpdateIcon()
        {
            _iconUpdateCts?.Cancel();
            _iconUpdateCts?.Dispose();
            
            _iconUpdateCts = new CancellationTokenSource();
            var token = _iconUpdateCts.Token;
            
            if (!_resourceManager.TryGetResourceIcon(_resourceType, out var iconRef))
            {
                Debug.LogError($"{GetType().Name} has no icon for {_resourceType}");
                return;
            }
            
            var icon = await iconRef
                .LoadAssetReference<Sprite>(iconRef.AssetGUID)
                .AsUniTask()
                .AttachExternalCancellation<Sprite>(token);
            
            _icon.sprite = icon;
        }

        public RectTransform GetRect()
        {
            return _icon.rectTransform;
        }

        private void OnDestroy()
        {
            _iconUpdateCts?.Cancel();
            _iconUpdateCts?.Dispose();
        }
    }
}

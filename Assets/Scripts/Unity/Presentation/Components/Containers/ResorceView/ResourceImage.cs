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
    public class ResourceImage : ResourceIconBase<Image>
    {
        
        protected override void UpdateSprite(Sprite sprite)
        {
            _icon.sprite = sprite;
        }

        public override RectTransform GetRect()
        {
            return _icon.rectTransform;
        }
    }
}

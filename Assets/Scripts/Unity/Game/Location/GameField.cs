using System;
using System.Collections.Generic;
using System.Linq;
using Environments.Common.Scripts;
using Unity.Infrastructure.Touch;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Unity.Game
{
    public class GameField : MonoBehaviour, ITouchHandler, ITouchTarget
    {
        public event Action<ITouchTarget, Vector2> OnClick;
        
        [SerializeField]
        private SpriteRenderer _roadSprite;
        [SerializeField]
        private BoxCollider2D _collider;
        
        [Inject] private ITouchController _touchController;
        
        private bool _isClickTarget = false;

        public void Initialize()
        {
            _touchController.AddHandler(this);
        }
        
        public void SetAsClickTarget(bool active)
        {
            _isClickTarget = active;
            ShowHideSelection(active);
        }
        
        private void ShowHideSelection(bool show)
        {
            var color = show ? Color.yellow : Color.white;
            _roadSprite.color = color;
        }
        
        public TouchHandlerType Type => TouchHandlerType.GameField;
        
        public bool OnTouchBegin(IReadOnlyCollection<TouchData> touch)
        {
            return true;
        }

        public void OnTouchEnd(IReadOnlyCollection<TouchData> touch)
        {
        }

        public void OnTouchMove(IReadOnlyCollection<TouchData> touch)
        {
            
        }

        public bool TryConsumeClick(TouchData touch)
        {
            if (_isClickTarget && IsUnderGameField(touch))
            {
                
                HandleClick(touch.MousePosition);
                Debug.Log($"OnPointerClick: {touch.MousePosition}");
                return true;
            }

            if (TryGetHitTarget(touch, out var target))
            {
                target.HandleClick(touch.MousePosition);
                return true;
            }
            
            return false;
        }

        public void HandleClick(Vector2 touchPosition)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(touchPosition);
            OnClick?.Invoke(this, worldPos);
        }
        
        private bool IsUnderGameField(TouchData touch)
        {
            var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(touch.MousePosition),
                Vector3.zero);
            return hits.Any(hit => hit.collider == _collider);
        }

        private void OnDestroy()
        {
            _touchController.RemoveHandler(this);
        }

        private bool TryGetHitTarget(TouchData touch, out ITouchTarget target)
        {
            target = default;
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.MousePosition),
                Vector3.zero);

            return hit.collider && 
                   (hit.collider.TryGetComponent(out target) || hit.collider.transform.parent.TryGetComponent(out target));
        }
    }
}
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
    public class GameField : MonoBehaviour, ITouchHandler
    {
        public event Action<Vector2> OnClick;
        
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
            ActivateHighLight(active);
        }
        
        private void ActivateHighLight(bool active)
        {
            var color = active ? Color.yellow : Color.white;
            _roadSprite.color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Vector2 inputPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            OnClick?.Invoke(inputPosition);
            Debug.Log($"OnPointerClick: {inputPosition}");
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
                Vector2 inputPosition = Camera.main.ScreenToWorldPoint(touch.MousePosition);
                OnClick?.Invoke(inputPosition);
                Debug.Log($"OnPointerClick: {inputPosition}");
                return true;
            }
            return false;
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

        /*private bool TryGetHitTarget(TouchData touch, out ClickDispatcherComponent target)
        {
            target = default;
            var hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(touch.MousePosition),
                Vector3.zero);

            return hit.collider && hit.collider.TryGetComponent(out target);
        }*/
    }
}
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unity.Game
{
    public class LocationRoad : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Vector2> OnClick;
        
        [SerializeField]
        private SpriteRenderer _roadSprite;
        
        public void ActivateHighLight(bool active)
        {
            var color = active ? Color.yellow : Color.white;
            _roadSprite.color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(eventData.position);
        }
    }
}
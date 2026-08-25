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
        
        public void ActivateRoadHighLight(bool active)
        {
            var color = active ? Color.white : Color.yellow;
            _roadSprite.color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(eventData.position);
        }
    }
}
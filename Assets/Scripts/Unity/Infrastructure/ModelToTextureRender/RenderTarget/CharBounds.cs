using System;
using UnityEngine;

namespace Exploration.Scripts.Controllers.ModelRender
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CharBounds : MonoBehaviour
    {
        
        [SerializeField, HideInInspector]
        private BoxCollider2D _boxCollider;

        public Bounds GetBounds() => _boxCollider.bounds;

        private void OnValidate()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _boxCollider.isTrigger = true;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Unity.Game
{
    public class MoveComponent2D : MoveComponentBase
    {
        public override void RotateTo(Vector3 targetPosition)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Sign(targetPosition.x - transform.position.x);
            transform.localScale = scale;
            
        }
        
        protected override void MoveToTarget(Vector3 targetPosition)
        {
            var direction = (targetPosition - transform.position).normalized;
            var delta = direction * (MoveSpeed * Time.deltaTime);
            transform.position += delta;
            _view.UpdateSortingByPosition();
        }
    }
}

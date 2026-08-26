using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Unity.Game
{
    public class MoveComponent2D : MoveComponentBase
    {
        public override void RotateTo(Vector3 targetPosition)
        {
            var direction = Mathf.Sign(targetPosition.x - transform.position.x);
            transform.localRotation = Quaternion.Euler(0, direction > 0 ? 0 : 180, 0);
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

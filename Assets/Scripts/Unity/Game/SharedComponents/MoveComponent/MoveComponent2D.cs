using UnityEngine;

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
            Vector2 targetPos = targetPosition;
            Vector2 direction = (targetPosition - transform.position).normalized;
            var delta = direction * (MoveSpeed * Time.deltaTime);
            Debug.Log(targetPosition);
            transform.position += (Vector3)delta;
        }
    }
}

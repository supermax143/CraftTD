using UnityEngine;

namespace Unity.Game
{
    public class MoveComponent2D : MoveComponentBase
    {
        public override void RotateTo(Vector3 targetPosition)
        {
            /*var direction = targetPosition - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }*/
            var scale = transform.localScale;
            scale.x = Mathf.Sign(targetPosition.x - transform.position.x);
            transform.localScale = scale;
            
        }
    }
}

using UnityEngine;

namespace Unity.Game
{
    public class MoveComponent3D : MoveComponentBase
    {
        public override void RotateTo(Vector3 targetPosition)
        {
            var lookDirection = targetPosition - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }
}

using UnityEngine;

namespace Unity.Game
{
    public class AttackTarget2D : AttackTargetBase<Collider2D>
    {
        public override Vector3 GetClosestPosition(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }
    }
}
using UnityEngine;

namespace Unity.Game
{
    public class AttackTarget2D : AttackTargetBase<Collider2D>
    {
        public override bool TryGetClosestPosition(Vector3 position, out Vector3 closestPosition)
        {
            closestPosition = default;
            if (_collider == null)
            {
                return false;
            }
            closestPosition = _collider.ClosestPoint(position);
            return true;
        }
    }
}
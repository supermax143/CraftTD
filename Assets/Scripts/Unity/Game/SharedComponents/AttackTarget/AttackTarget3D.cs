using UnityEngine;

namespace Unity.Game
{
    public class AttackTarget3D : AttackTargetBase<Collider>
    {
        public override Vector3 GetClosestPosition(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }
    }
}
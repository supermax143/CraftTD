using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{
    public abstract class MoveComponentBase : GameComponent
    {
        [SerializeField]
        private MoveSpeedAttribute _moveSpeed = new MoveSpeedAttribute(4);

        private Vector3 _targetPosition;
        private bool _moving = false;

        public void StartMove(AttackTargetBase target)
        {
            _targetPosition = target.GetClosestPosition(transform.position);
            _moving = true;
        }

        public void StopMove()
        {
            _moving = false;
        }

        private void Update()
        {
            if (!_moving)
            {
                return;
            }
            MoveToTarget(_targetPosition);
            RotateTo(_targetPosition);
        }

        private void MoveToTarget(Vector3 targetPosition)
        {
            var direction = (targetPosition - transform.position).normalized;
            var delta = direction * (_moveSpeed.ValueModified * Time.deltaTime);
            delta.y = 0;
            transform.position += delta;
        }

        public abstract void RotateTo(Vector3 targetPosition);
    }
}

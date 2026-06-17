using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{
    public abstract class MoveComponentBase : GameComponent
    {
        [SerializeField]
        private MoveSpeedAttribute _moveSpeed = new MoveSpeedAttribute(4);

        private bool _moving = false;
        private AttackTargetBase _target;

        public float MoveSpeed => _moveSpeed.ValueModified;
         public Vector3 TargetPosition => _target.GetClosestPosition(transform.position);
        //public Vector3 TargetPosition => _target.transform.position;
        
        public bool IsMoving => _moving;

        public void StartMove(AttackTargetBase target)
        {
            _target = target;
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

            var pos = TargetPosition;
            MoveToTarget(pos);
            RotateTo(pos);
        }

       
        
        protected virtual void MoveToTarget(Vector3 targetPosition)
        {
            var direction = (targetPosition - transform.position).normalized;
            var delta = direction * (_moveSpeed.ValueModified * Time.deltaTime);
            transform.position += delta;
        }

        public abstract void RotateTo(Vector3 targetPosition);
    }
}

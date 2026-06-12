using System;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{
    public class MoveComponent : GameComponent
    {
        [SerializeField]
        private MoveSpeedAttribute _moveSpeed = new MoveSpeedAttribute(4);
        
        
        private Vector3 _targetPosition;
        private bool _moving = false;


        public void StartMove(AttackTarget target)
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

        public void RotateTo(Vector3 targetPosition)
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
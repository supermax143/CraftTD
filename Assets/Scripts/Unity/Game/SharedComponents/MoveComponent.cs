using System;
using UnityEngine;

namespace Unity.Game
{
    public class MoveComponent : MonoBehaviour
    {
        private MoveData _data;
        private AttackTarget _target;

        public MoveData Data => _data;

        private bool _moving = false;
        
        private Vector3 _targetPosition;

        public void Initialize(MoveData data)
        {
            _data = data;   
        }

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
            var delta = direction * (Data.Speed * Time.deltaTime);
            delta.y = 0;
            transform.position += delta;
            
            /*var lookDirection = targetPosition - _unit.transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                _unit.transform.rotation = Quaternion.LookRotation(lookDirection);
            }*/
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
using System;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{
    public abstract class MoveComponentBase : GameComponent
    {
        [SerializeField]
        private MoveSpeedAttribute _moveSpeed = new MoveSpeedAttribute(4);
        [SerializeField, HideInInspector]
        protected UnitView _view;
        
        private bool _moving = false;
        private AttackTargetBase _target;

        public float MoveSpeed => _moveSpeed.BaseValueModified;

        public bool TryTargetPosition(out Vector3 pos) 
            => _target.TryGetClosestPosition(transform.position, out pos);
        
        public bool IsMoving => _moving;

        private void OnValidate()
        {
            _view = GetComponentInChildren<UnitView>();
        }


        public void StartMove(AttackTargetBase target)
        {
            _target = target;
            _view.StartWalking();
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

            if (!TryTargetPosition(out var pos))
            {
                return;
            }
            MoveToTarget(pos);
            RotateTo(pos);
        }

       
        
        protected virtual void MoveToTarget(Vector3 targetPosition)
        {
            var direction = (targetPosition - transform.position).normalized;
            var delta = direction * (_moveSpeed.BaseValueModified * Time.deltaTime);
            transform.position += delta;
        }

        public abstract void RotateTo(Vector3 targetPosition);
    }
}

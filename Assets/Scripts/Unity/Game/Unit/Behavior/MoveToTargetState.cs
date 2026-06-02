using System.Linq;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние движения к выбранной цели
    /// </summary>
    public class MoveToTargetState : UnitState
    {
        
        private readonly Timer _detectionTimer = new();

        public override void Enter()
        {
            _detectionTimer.Start(_unit.TargetSearch.Data.DetectionInterval);
            _unit.Move.StartMove(_stateManager.CurrentTarget);
        }

        public override void UpdateState()
        {
            if (_stateManager.CurrentTarget == null)
            {
                ChangeState<SearchTargetState>();
                return;
            }

            if (_unit.Attack.CheckRange(_stateManager.CurrentTarget))
            {
                ChangeState<AttackTargetState>();
                return;
            }

            if (_detectionTimer.IsComplete && 
                _unit.TargetSearch.TryGetClosestTarget(out var newTarget))
            {
                if (_stateManager.CurrentTarget != newTarget)
                {
                    _stateManager.CurrentTarget = newTarget;
                    _unit.Move.StartMove(_stateManager.CurrentTarget);
                }
                
                _detectionTimer.Start(_unit.TargetSearch.Data.DetectionInterval);
            }
            
            
            //MoveToTarget(_stateManager.CurrentTarget);
        }

        public override void Exit()
        {
            _unit.Move.StopMove();
        }

        /*private void MoveToTarget(AttackTarget target)
        {
            var targetPosition = target.GetClosestPosition(_unit.transform.position);
            var direction = (targetPosition - _unit.transform.position).normalized;
            var delta = direction * (_unit.Move.Data.Speed * Time.deltaTime);
            delta.y = 0;
            _unit.transform.position += delta;
            
            var lookDirection = targetPosition - _unit.transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                _unit.transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }*/
        
    }
}

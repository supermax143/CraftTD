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
            _detectionTimer.Start(TargetSearchComponent.DETECTION_INTERVAL);
            _unit.MoveComponent.StartMove(_stateManager.CurrentTarget);
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
                _unit.TargetSearchComponent.TryGetClosestTarget(out var newTarget))
            {
                if (_stateManager.CurrentTarget != newTarget)
                {
                    _stateManager.CurrentTarget = newTarget;
                    _unit.MoveComponent.StartMove(_stateManager.CurrentTarget);
                }
                
                _detectionTimer.Start(TargetSearchComponent.DETECTION_INTERVAL);
            }
            
            
            //MoveToTarget(_stateManager.CurrentTarget);
        }

        public override void Exit()
        {
            _unit.MoveComponent.StopMove();
        }
        
        
    }
}

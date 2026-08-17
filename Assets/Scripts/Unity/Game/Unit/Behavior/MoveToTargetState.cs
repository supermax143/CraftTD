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
        
        private enum PointSide
        {
            Left,
            Right
        }
        
        private readonly Timer _detectionTimer = new();

        
        
        
        private static PointSide GetPointSide(BoxCollider2D box, Vector2 point)
        {
            Vector2 localPoint = box.transform.InverseTransformPoint(point);

            return localPoint.x < box.offset.x
                ? PointSide.Left
                : PointSide.Right;
        }
        
        private BoxCollider2D _towerDoorCollider;

        private bool CheckIfOutOfTower()
        {
            var position = _unit.TargetSearchComponent.SearchTransform.position;
            var pointSide = GetPointSide(_towerDoorCollider, position);
            return pointSide == PointSide.Right;
        }

        private bool CheckCanAttack()
        {
            if (_unit.Faction == Faction.Enemy && !CheckIfOutOfTower())
            {
                return false;
            }

            return _unit.Attack.CheckRange(_stateManager.CurrentTarget);
        }
        
        public override void Enter()
        {
            if (_stateManager.GameController.TryGetTower(_unit.Faction, out var tower))
            {
                _towerDoorCollider = tower.GetComponent<BoxCollider2D>();
            }
            _detectionTimer.Start(TargetSearchComponentBase.DETECTION_INTERVAL);
            _unit.MoveComponent.StartMove(_stateManager.CurrentTarget);
        }

        public override void UpdateState()
        {
            Debug.Log($"out of tower: {CheckIfOutOfTower()}");
            
            if (_stateManager.CurrentTarget == null)
            {
                ChangeState<SearchTargetState>();
                return;
            }

            if (CheckCanAttack())
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
                
                _detectionTimer.Start(TargetSearchComponentBase.DETECTION_INTERVAL);
            }
        }

        public override void Exit()
        {
            base.Exit();
            _unit.MoveComponent.StopMove();
            _detectionTimer.Stop();
        }
        
        
    }
}

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
            _detectionTimer.Start(_unit.DetectionInterval);
        }

        public override void UpdateState()
        {
            if (_stateManager.CurrentTarget == null)
            {
                ChangeState<MoveToTowerState>();
                return;
            }

            if (_unit.Attack.CheckRange(_stateManager.CurrentTarget))
            {
                ChangeState<AttackTargetState>();
                return;
            }

            if (_detectionTimer.IsComplete)
            {
                CheckForNearTargets();
                _detectionTimer.Start(_unit.DetectionInterval);
            }
            
            MoveToTarget(_stateManager.CurrentTarget);
        }

        private void MoveToTarget(AttackTarget target)
        {
            var targetPosition = target.GetClosestPosition(_unit.transform.position);
            var direction = (targetPosition - _unit.transform.position).normalized;
            var delta = direction * _unit.MoveSpeed * Time.deltaTime;
            delta.y = 0;
            _unit.transform.position += delta;
            
            var lookDirection = targetPosition - _unit.transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                _unit.transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
        
        private void CheckForNearTargets()
        {
            var colliders = Physics.OverlapSphere(_unit.transform.position, _unit.DetectionRange)
                .OrderByDescending(x => Vector3.Distance(x.transform.position, _unit.transform.position))
                .ToArray();
            
            foreach (var collider in colliders)
            {
                var attackTarget = collider.GetComponent<AttackTarget>();
                if (attackTarget != null && !attackTarget.IsDead && attackTarget.Faction == _unit.OpponentFaction)
                {
                    _stateManager.CurrentTarget = attackTarget;
                    return;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_unit == null) return;
            
            Gizmos.color = Color.blue;
            DrawCircle(_unit.transform.position, _unit.DetectionRange);
            
            if (_unit.Attack.Data != null)
            {
                Gizmos.color = Color.red;
                DrawCircle(_unit.transform.position, _unit.Attack.Data.Range);
            }
        }

        private void DrawCircle(Vector3 center, float radius)
        {
            const int segments = 32;
            var angleStep = 360f / segments;
            
            for (int i = 0; i < segments; i++)
            {
                var angle1 = i * angleStep * Mathf.Deg2Rad;
                var angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;
                
                var point1 = center + new Vector3(Mathf.Cos(angle1) * radius, 0, Mathf.Sin(angle1) * radius);
                var point2 = center + new Vector3(Mathf.Cos(angle2) * radius, 0, Mathf.Sin(angle2) * radius);
                
                Gizmos.DrawLine(point1, point2);
            }
        }
        
    }
}

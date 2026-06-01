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
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _detectionRadius = 2f;
        [SerializeField] private float _detectionInterval = .5f;
        
        private readonly Timer _detectionTimer = new();

        public override void Enter()
        {
            _detectionTimer.Start(_detectionInterval);
        }

        public override void Update()
        {
            if (_stateManager.CurrentTarget == null)
            {
                ChangeState<MoveToTowerState>();
                return;
            }

            var targetPosition = _stateManager.CurrentTarget.GetClosestPosition(_unit.transform.position);
            var distance = Vector3.Distance(_unit.transform.position, targetPosition);
            var attackRange = _unit.Attack.Range;

            if (distance <= attackRange)
            {
                ChangeState<AttackTargetState>();
                return;
            }

            if (_detectionTimer.IsComplete)
            {
                CheckForNearTargets();
                _detectionTimer.Start(_detectionInterval);
            }
            
            MoveToTarget(targetPosition);
        }

        private void MoveToTarget(Vector3 targetPosition)
        {
            var direction = (targetPosition - _unit.transform.position).normalized;
            var delta = direction * _moveSpeed * Time.deltaTime;
            delta.y = 0;
            _unit.transform.position += delta;
            _unit.transform.LookAt(targetPosition);
        }
        
        private void CheckForNearTargets()
        {
            var colliders = Physics.OverlapSphere(_unit.transform.position, _detectionRadius)
                .OrderByDescending(x => Vector3.Distance(x.transform.position, _unit.transform.position))
                .ToArray();
            
            foreach (var collider in colliders)
            {
                var attackTarget = collider.GetComponent<AttackTarget>();
                if (attackTarget != null && attackTarget.Faction == _unit.OpponentFaction)
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
            DrawCircle(_unit.transform.position, _detectionRadius);
            
            if (_unit.Attack != null)
            {
                Gizmos.color = Color.red;
                DrawCircle(_unit.transform.position, _unit.Attack.Range);
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

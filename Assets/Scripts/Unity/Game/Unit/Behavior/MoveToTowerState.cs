using System.Linq;
using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние движения к вражеской башне с поиском целей
    /// </summary>
    public class MoveToTowerState : UnitState
    {
        
        private float _detectionTimer;
        private AttackTarget _targetTower;
        

        public override void Enter()
        {
            _detectionTimer = 0f;
            FindTargetTower();
        }

        public override void UpdateState()
        {
            if (_targetTower == null)
            {
                FindTargetTower();
                if (_targetTower == null) return;
            }

            MoveToTarget(_targetTower.transform.position);
            
            _detectionTimer += Time.deltaTime;
            if (_detectionTimer >= _unit.DetectionInterval)
            {
                _detectionTimer = 0f;
                CheckForTargets();
            }
        }

        private void FindTargetTower()
        {
            if (_stateManager.GameController.TryGetOpponentTower(_unit.OpponentFaction, out var tower))
            {
                _targetTower = tower;
            }
        }

        private void MoveToTarget(Vector3 targetPosition)
        {
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

        private void CheckForTargets()
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
                    ChangeState<MoveToTargetState>();
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

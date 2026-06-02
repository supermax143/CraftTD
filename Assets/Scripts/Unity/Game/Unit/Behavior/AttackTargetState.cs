using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние атаки выбранной цели
    /// </summary>
    public class AttackTargetState : UnitState
    {
        private float _attackTimer;

        public override void Enter()
        {
            _attackTimer = 0f;
        }

        public override void Update()
        {
            if (_stateManager.CurrentTarget == null)
            {
                ChangeState<MoveToTowerState>();
                return;
            }

            /*var targetPosition = _stateManager.CurrentTarget.GetClosestPosition(_unit.transform.position);
            var distance = Vector3.Distance(_unit.transform.position, targetPosition);*/
            if (!_unit.Attack.CheckRange(_stateManager.CurrentTarget))
            {
                ChangeState<MoveToTargetState>();
                return;
            }

            _unit.transform.LookAt(_stateManager.CurrentTarget.transform);
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _unit.Attack.Data.Cooldown)
            {
                _attackTimer = 0f;
                Attack();
            }
        }

        private void Attack()
        {
            if (_stateManager.CurrentTarget == null)
            {
                return;
            }
            
            _stateManager.CurrentTarget.Health.TakeDamage(_unit.Attack.Data.Damage);
        }

        private void OnDrawGizmos()
        {
            if (_unit == null || _unit.Attack.Data == null) return;
            
            Gizmos.color = Color.red;
            DrawCircle(_unit.transform.position, _unit.Attack.Data.Range);
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

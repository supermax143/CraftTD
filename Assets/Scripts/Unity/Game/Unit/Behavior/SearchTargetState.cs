using System.Linq;
using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние Поиска цели
    /// </summary>
    public class SearchTargetState : UnitState
    {
        
        public override void Enter()
        {
            if (TryGetTarget(out var target) || TryGetTargetTower(out target))
            {
                _stateManager.CurrentTarget = target;
                ChangeState<MoveToTargetState>();
                return;
            }
            
            Debug.LogError($"{this.GetType().Name} Target not found");
        }

        private bool TryGetTarget(out AttackTarget target)
        {
            var colliders = Physics.OverlapSphere(_unit.transform.position, _unit.DetectionRange)
                .OrderByDescending(x => Vector3.Distance(x.transform.position, _unit.transform.position))
                .ToArray();
            
            target = colliders.
                Select(c => c.GetComponent<AttackTarget>()).
                FirstOrDefault(t => t != null && !t.IsDead && t.Faction == _unit.OpponentFaction);
            
            return  target != null;
        }
        
        private bool TryGetTargetTower(out AttackTarget target)
            => _stateManager.GameController.TryGetOpponentTower(_unit.OpponentFaction, out target);

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

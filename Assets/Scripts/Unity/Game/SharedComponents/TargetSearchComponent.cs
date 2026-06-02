using System.Linq;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class TargetSearchComponent : MonoBehaviour
    {
        
        [Inject] private GameController _gameController;
        
        private TargetSearchData _data;
        private UnitController _unit;

        public Transform SearchTransform => transform;

        public TargetSearchData Data => _data;

        public void Initialize(TargetSearchData data, UnitController unit)
        {
            _data = data;   
            _unit = unit;
        }
        
        
        
        public bool TryGetClosestTarget(out AttackTarget target)
        {
            var colliders = Physics.OverlapSphere(SearchTransform.position, _data.DetectionRange)
                .OrderByDescending(x => Vector3.Distance(x.transform.position, SearchTransform.position))
                .ToArray();
            
            target = null;
            
            foreach (var c in colliders)
            {
                var t = c.GetComponent<AttackTarget>();
                if (t != null && !t.IsDead && t.Faction == _unit.OpponentFaction)
                {
                    target = t;
                    break;
                }
            }
            
            return  target != null;
        }
        
        public bool TryGetTargetTower(out AttackTarget target)
            => _gameController.TryGetOpponentTower(_unit.OpponentFaction, out target);

        private void OnDrawGizmos()
        {
            if (_unit == null) return;
            
            Gizmos.color = Color.blue;
            DrawCircle(_unit.transform.position, _unit.TargetSearch.Data.DetectionRange);
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
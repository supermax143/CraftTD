using System.Linq;
using Sirenix.OdinInspector;
using Unity.Game.Attributes.Specific;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class TargetSearchComponent : GameEntity
    {
        public const float DETECTION_INTERVAL = .3f;
        
        
        [Inject] private IGameController _gameController;

        [Inject] private GameStats _gameStats;
        /*private TargetSearchData _data;
        private UnitController _unit;*/

        [SerializeField] 
        private AttackRangeAttribute _attackRange;
        


        public Transform SearchTransform => transform;
        public float DetectionRange => _gameStats.DetectionRange;
        public float AttackRange => _attackRange.ValueModified;
        
        
        
        // public TargetSearchData Data => _data;
        private Faction _faction;
        private Faction _opponentFaction;

        public void SetFaction(Faction faction, Faction opponentFaction)
        {
            _faction = faction;
            _opponentFaction = opponentFaction;
        }
        
        /*public void Initialize(TargetSearchData data, UnitController unit)
        {
            _data = data;   
            _unit = unit;
        }*/
        
        
        public bool TryGetClosestTarget(out AttackTarget target)
        {
            var colliders = Physics.OverlapSphere(SearchTransform.position, DetectionRange)
                .OrderByDescending(x => Vector3.Distance(x.transform.position, SearchTransform.position))
                .ToArray();
            
            target = null;
            
            foreach (var c in colliders)
            {
                var t = c.GetComponent<AttackTarget>();
                if (t != null && !t.IsDead && t.Faction == _opponentFaction)
                {
                    target = t;
                    break;
                }
            }
            
            return  target != null;
        }
        
        public bool TryGetTargetTower(out AttackTarget target)
            => _gameController.TryGetOpponentTower(_opponentFaction, out target);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            DrawCircle(transform.position, DetectionRange);
            Gizmos.color = Color.red;
            DrawCircle(transform.position, AttackRange);
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
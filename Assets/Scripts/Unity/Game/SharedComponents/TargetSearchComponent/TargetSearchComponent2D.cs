using System.Linq;
using UnityEngine;

namespace Unity.Game
{
    public class TargetSearchComponent2D : TargetSearchComponentBase
    {
        public override bool TryGetClosestTarget(out AttackTargetBase target)
        {
            target = Physics2D.OverlapCircleAll(SearchTransform.position, DetectionRange)
                .Select(c => c.GetComponent<AttackTargetBase>())
                .Where(t => t != null && !t.IsDead && t.Faction == _opponentFaction)
                .OrderBy(t => Vector2.Distance(t.transform.position, SearchTransform.position))
                .FirstOrDefault();
         
            return target != null;
        }

        protected override void DrawCircle(Vector3 center, float radius)
        {
            const int segments = 32;
            var angleStep = 360f / segments;

            for (int i = 0; i < segments; i++)
            {
                var angle1 = i * angleStep * Mathf.Deg2Rad;
                var angle2 = (i + 1) * angleStep * Mathf.Deg2Rad;

                var point1 = center + new Vector3(Mathf.Cos(angle1) * radius, Mathf.Sin(angle1) * radius, 0);
                var point2 = center + new Vector3(Mathf.Cos(angle2) * radius, Mathf.Sin(angle2) * radius, 0);

                Gizmos.DrawLine(point1, point2);
            }
        }
    }
}

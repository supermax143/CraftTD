using System.Linq;
using UnityEngine;

namespace Unity.Game
{
    public class TargetSearchComponent3D : TargetSearchComponentBase
    {
        public override bool TryGetClosestTarget(out AttackTargetBase target)
        {
            var colliders = Physics.OverlapSphere(SearchTransform.position, DetectionRange)
                .OrderByDescending(x => Vector3.Distance(x.transform.position, SearchTransform.position))
                .ToArray();

            target = null;

            foreach (var c in colliders)
            {
                var t = c.GetComponent<AttackTargetBase>();
                if (t != null && !t.IsDead && t.Faction == _opponentFaction)
                {
                    target = t;
                    break;
                }
            }

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

                var point1 = center + new Vector3(Mathf.Cos(angle1) * radius, 0, Mathf.Sin(angle1) * radius);
                var point2 = center + new Vector3(Mathf.Cos(angle2) * radius, 0, Mathf.Sin(angle2) * radius);

                Gizmos.DrawLine(point1, point2);
            }
        }
    }
}

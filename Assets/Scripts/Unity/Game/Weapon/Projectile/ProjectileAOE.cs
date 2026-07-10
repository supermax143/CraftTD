using System.Linq;
using UnityEngine;

namespace Unity.Game.Projectile
{
    public class ProjectileAOE : ProjectileBase
    {
        [SerializeField]
        private float _damageRadius;


        protected override void ApplyDamage(Vector3 startPosition)
        {
            base.ApplyDamage(startPosition);
            
            var colliders = Physics2D.OverlapCircleAll(transform.position, _damageRadius);
            
            var targets = colliders
                .Select(c => c.GetComponent<AttackTargetBase>())
                .Where(t => t != null && !t.IsDead && t.Faction == _target.Faction && t != _target);

            foreach (var target in targets)
            {
                target.HealthComponent.TakeDamage(_damage * 0.1f);
            }
        }

    }
}
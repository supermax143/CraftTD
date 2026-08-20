using UnityEngine;

namespace Unity.Game.Projectile
{
    public class MeleeWeapon : Weapon
    {
        public override void Attack(AttackTargetBase target, float damage, Vector3 direction)
        {
            target.SetLastAttackDirection(direction);
            target.HealthComponent.TakeDamage(damage);
            if (target.TryGetAttackPosition(true, out var position))
            {
                PopupSpawnManager.SpawnRandomHitBubble(position, null,
                    new Vector2(0,0), new Vector2(0,1));
            }
        }
    }
}
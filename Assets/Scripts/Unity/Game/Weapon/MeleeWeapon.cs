using UnityEngine;

namespace Unity.Game.Projectile
{
    public class MeleeWeapon : Weapon
    {
        public override void Attack(AttackTargetBase target, float damage, Vector3 direction)
        {
            target.SetLastAttackDirection(direction);
            target.HealthComponent.TakeDamage(damage);
        }
    }
}
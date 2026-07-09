using UnityEngine;

namespace Unity.Game.Projectile
{
    public class RangedWeapon : Weapon
    {
        [SerializeField]
        private Transform _barrel;
        [SerializeField]
        private GameObject _projectilePrefab;
        
        
        public override void Attack(AttackTargetBase target, float damage, Vector3 direction)
        {
            if (_projectilePrefab == null)
            {
                ApplyDamage(target, damage);
            }
            else
            {
                var projectile = Instantiate(_projectilePrefab, _barrel.position, _barrel.rotation);
                projectile.GetComponent<ProjectileComponent>().Launch(target, damage);
            }
        }

        private void ApplyDamage(AttackTargetBase target, float damage)
        {
            if (target != null && target.HealthComponent != null)
            {
                target.HealthComponent.TakeDamage(damage);
            }
        }
    }
}
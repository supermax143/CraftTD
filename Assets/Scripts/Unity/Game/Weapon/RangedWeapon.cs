using UnityEngine;

namespace Unity.Game.Projectile
{
    public class RangedWeapon : Weapon
    {
        [SerializeField]
        private Transform _barrel;
        [SerializeField]
        private GameObject _projectilePrefab;
        
        
        public override void Attack(AttackTargetBase target, float damage)
        {
            var projectile = Instantiate(_projectilePrefab, _barrel.position, _barrel.rotation);
            projectile.GetComponent<ProjectileComponent>().Launch(target, 15, damage);
        }
    }
}
using Cysharp.Threading.Tasks.Triggers;
using Unity.Infrastructure.Effects;
using UnityEngine;
using Zenject;

namespace Unity.Game.Projectile
{
    public class RangedWeapon : Weapon
    {
        [SerializeField]
        private Transform _barrel;
        [SerializeField]
        private GameObject _projectilePrefab;
        [SerializeField]
        private PopupType _instantDamageEffect;
        
        public override void Attack(AttackTargetBase target, float damage, Vector3 direction)
        {
            if (_projectilePrefab == null)
            {
                ApplyInstantDamage(target, damage);
            }
            else
            {
                var projectile = Instantiate(_projectilePrefab, _barrel.position, _barrel.rotation).GetComponent<ProjectileBase>();
                projectile.Launch(target, damage, PopupSpawnManager);
            }
        }

        private void ApplyInstantDamage(AttackTargetBase target, float damage)
        {
            if (target != null && target.HealthComponent != null)
            {
                if (_instantDamageEffect != PopupType.None &&
                    target.TryGetAttackPosition(true, out var effectPosition))
                {
                    PopupSpawnManager.SpawnEffect(_instantDamageEffect, effectPosition);
                }
               
                target.HealthComponent.TakeDamage(damage);
            }
        }
    }
}
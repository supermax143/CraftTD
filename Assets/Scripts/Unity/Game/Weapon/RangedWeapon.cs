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
        private VisualEffectType _instantDamageEffect;
        
        [Inject] private VisualEffectSpawnManager _visualEffectSpawnManager;
        
        public override void Attack(AttackTargetBase target, float damage, Vector3 direction)
        {
            if (_projectilePrefab == null)
            {
                ApplyInstantDamage(target, damage);
            }
            else
            {
                var projectile = Instantiate(_projectilePrefab, _barrel.position, _barrel.rotation).GetComponent<ProjectileBase>();
                projectile.Launch(target, damage, _visualEffectSpawnManager);
            }
        }

        private void ApplyInstantDamage(AttackTargetBase target, float damage)
        {
            if (target != null && target.HealthComponent != null)
            {
                if (_instantDamageEffect != VisualEffectType.None &&
                    target.TryGetAttackPosition(true, out var effectPosition))
                {
                    _visualEffectSpawnManager.SpawnEffect(_instantDamageEffect, effectPosition);
                }
               
                target.HealthComponent.TakeDamage(damage);
            }
        }
    }
}
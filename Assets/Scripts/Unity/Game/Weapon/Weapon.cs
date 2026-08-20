using Unity.Infrastructure.Effects;
using UnityEngine;
using Zenject;

namespace Unity.Game.Projectile
{
    public abstract class Weapon : MonoBehaviour
    {
        [Inject] protected VisualEffectSpawnManager _visualEffectSpawnManager;
        
        public abstract void Attack(AttackTargetBase target, float damage, Vector3 direction);
    }
}
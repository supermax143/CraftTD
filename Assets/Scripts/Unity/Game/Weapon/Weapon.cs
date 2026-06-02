using UnityEngine;

namespace Unity.Game.Projectile
{
    public abstract class Weapon : MonoBehaviour
    {
        public abstract void Attack(AttackTarget target, float damage);
    }
}
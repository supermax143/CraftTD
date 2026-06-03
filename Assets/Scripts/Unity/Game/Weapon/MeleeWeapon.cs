namespace Unity.Game.Projectile
{
    public class MeleeWeapon : Weapon
    {
        public override void Attack(AttackTarget target, float damage)
        {
            target.HealthComponent.TakeDamage(damage);
        }
    }
}
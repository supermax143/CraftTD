namespace Unity.Game.Projectile
{
    public class MeleeWeapon : Weapon
    {
        public override void Attack(AttackTarget target, float damage)
        {
            target.Health.TakeDamage(damage);
        }
    }
}
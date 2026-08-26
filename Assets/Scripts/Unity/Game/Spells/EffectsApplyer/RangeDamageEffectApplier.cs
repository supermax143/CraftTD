using System.Linq;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game.Spells.EffectsApplyer
{
    public class RangeDamageEffectApplier : SpellEffectApplier
    {
        [SerializeField] 
        private DamageAttribute _damage;
        [SerializeField] 
        private float _radius = 2;
        
        public override void Apply(SpellController spell)
        {
            foreach (var modifier in spell.Model.GetModifiers(GetHashCode()))
            {
                _damage.AddModifier(modifier);
            }
                
            var colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
            var targets = colliders
                .Select(c => c.GetComponent<AttackTargetBase>())
                .Where(t => t != null && !t.IsDead);//add faction check
                //.Where(t => t != null && !t.IsDead && t.Faction == _target.Faction && t != _target);

            foreach (var target in targets)
            {
                target.HealthComponent.TakeDamage(_damage.BaseValueModified);
            }
        }
    }
}
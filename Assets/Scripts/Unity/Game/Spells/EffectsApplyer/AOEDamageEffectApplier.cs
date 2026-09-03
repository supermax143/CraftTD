using System.Linq;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game.Spells.EffectsApplyer
{
    public class AOEDamageEffectApplier : SpellEffectApplier
    {
        [SerializeField] 
        private DamageAttribute _damage;
        [SerializeField] 
        private AttackRangeAttribute _radius;
        [SerializeField] 
        private Faction _faction = Faction.Enemy;
        
        public override void Apply(SpellController spell)
        {
            foreach (var modifier in spell.Model.GetModifiers())
            {
                _damage.AddModifier(modifier);
                _radius.AddModifier(modifier);
            }
            
            var position = spell.ActivationComponent.TargetPosition;
            var colliders = Physics2D.OverlapCircleAll(position, _radius.BaseValueModified);
            var targets = colliders
                .Select(c => c.GetComponent<AttackTargetBase>())
                .Where(t => t != null && !t.IsDead && t.Faction == _faction)
                .Distinct();

            foreach (var target in targets)
            {
                target.HealthComponent.TakeDamage(_damage.BaseValueModified);
            }
        }
    }
}
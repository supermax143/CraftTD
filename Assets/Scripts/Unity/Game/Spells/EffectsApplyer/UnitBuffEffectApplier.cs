using System.Linq;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game.Spells.EffectsApplyer
{
    public class UnitBuffEffectApplier : SpellEffectApplier
    {
        
        [SerializeField] 
        private float _radius = 2;
        
        public override void Apply(SpellController spell)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
            var targets = colliders
                .Select(c => c.GetComponent<AttackTargetBase>())
                .Where(t => t != null && !t.HealthComponent.IsDead && t.transform.parent.TryGetComponent<UnitController>( out _))
                .Select(t => t.transform.parent.GetComponent<UnitController>())
                .Distinct();
            //.Where(t => t != null && !t.IsDead && t.Faction == _target.Faction && t != _target);

            var modifiers = spell.Model.GetModifiers();
            foreach (var target in targets)
            {
                target.transform.localScale *= 2;
                target.ApplyModifiers(modifiers);
            }
        }
    }
}
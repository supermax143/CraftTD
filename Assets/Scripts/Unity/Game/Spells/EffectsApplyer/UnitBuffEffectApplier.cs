using System.Linq;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game.Spells.EffectsApplyer
{
    public class UnitBuffEffectApplier : SpellEffectApplier
    {
        
        public override void Apply(SpellController spell)
        {
            var target = spell.ActivationComponent.Target as UnitController;
            if (target != null)
            {
                target.transform.localScale *= 1.5f;
                target.ApplyModifiers(spell.Model.GetModifiers());
            }
        }
    }
}
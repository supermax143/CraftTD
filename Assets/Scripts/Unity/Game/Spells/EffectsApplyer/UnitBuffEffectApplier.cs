using System.Linq;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game.Spells.EffectsApplyer
{
    public class UnitBuffEffectApplier : SpellEffectApplier
    {
        
        public override void Apply(SpellController spell)
        {
            var targets = spell.ActivationComponent.Target as UnitController;
            if (targets != null)
            {
                targets.transform.localScale *= 1.5f;
                targets.ApplyModifiers(spell.Model.GetModifiers());
            }
        }
    }
}
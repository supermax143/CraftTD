using System.Collections;
using UnityEngine;

namespace Unity.Game.Spells.Execution
{
    public class InstantExecutionComponent : SpellExecutionComponent
    {
        protected override IEnumerator DoExecute(SpellController spell)
        {
            spell.EffectApplier.Apply(spell);
            yield return new WaitForSeconds(_delayBeforeDestroy);
        }
    }
}
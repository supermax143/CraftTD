using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.Spells.EffectsApplyer
{
    public abstract class SpellEffectApplier : MonoBehaviour
    {
        public event  Action OnEffectApplied;
        
        public void Apply(SpellController spell)
        {
            StartCoroutine(WaitEffectApplied(spell));
        }

        private IEnumerator WaitEffectApplied(SpellController spell)
        {
            yield return DoApply(spell);
            OnEffectApplied?.Invoke();
        }
        
        protected abstract IEnumerator DoApply(SpellController spell);
    }
}
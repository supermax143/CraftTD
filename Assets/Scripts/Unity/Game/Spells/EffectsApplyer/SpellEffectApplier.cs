using UnityEngine;

namespace Unity.Game.Spells.EffectsApplyer
{
    public abstract class SpellEffectApplier : MonoBehaviour
    {
        public abstract void Apply(SpellController spell);
    }
}
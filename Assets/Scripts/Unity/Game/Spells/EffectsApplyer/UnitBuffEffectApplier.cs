using System.Linq;
using Unity.Game.Attributes.Specific;
using Unity.Infrastructure.Effects;
using UnityEngine;
using Zenject;

namespace Unity.Game.Spells.EffectsApplyer
{
    public class UnitBuffEffectApplier : SpellEffectApplier
    {
        [Inject] private PopupSpawnManager _popupSpawnManager;
        
        public override async void Apply(SpellController spell)
        {
            var target = spell.ActivationComponent.Target as UnitController;
            if (target != null)
            {
                var effect = await _popupSpawnManager.SpawnEffect(PopupType.RingGlowEffect, new Vector3(0,0,.1f), target.View.EffectsContainerMiddle);
                effect.transform.localPosition = Vector3.zero;
                target.ApplyModifiers(spell.Model.GetModifiers());
                target.View.SetScale(1.5f, .7f);
            }
        }
    }
}
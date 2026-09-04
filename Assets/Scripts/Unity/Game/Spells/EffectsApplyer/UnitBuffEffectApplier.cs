using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Game.Attributes.Specific;
using Unity.Infrastructure.Effects;
using UnityEngine;
using Zenject;

namespace Unity.Game.Spells.EffectsApplyer
{
    public class UnitBuffEffectApplier : SpellEffectApplier
    {
        [Inject] private PopupSpawnManager _popupSpawnManager;
        
        private bool _onEffectShowCompleted = false;
        
        protected override  IEnumerator DoApply(SpellController spell)
        {
            var target = spell.ActivationComponent.Target as UnitController;
            if (target != null)
            {
                /*var effect = await _popupSpawnManager.SpawnEffect(PopupType.RingGlowEffect, new Vector3(0,0,.1f), target.View.EffectsContainerMiddle);
                effect.transform5.localPosition = Vector3.zero;*/
                ShowEffect(target).Forget();
                yield return new WaitUntil(() => _onEffectShowCompleted);
                target.ApplyModifiers(spell.Model.GetModifiers());
                target.View.SetScale(1.5f, .7f);
            }
        }
        
        private async UniTask ShowEffect(UnitController target)
        {
            var effect = await _popupSpawnManager.SpawnEffect(PopupType.RingGlowEffect, new Vector3(0,0,.1f), target.View.EffectsContainerMiddle);
            effect.transform.localPosition = Vector3.zero;
            _onEffectShowCompleted = true;
        }
    }
}
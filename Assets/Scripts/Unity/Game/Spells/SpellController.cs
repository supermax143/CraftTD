using System;
using Core.Application.Spells;
using Core.Application.Spells.Activation;
using Core.Application.Spells.Targeting;
using Unity.Game.Spells.EffectsApplyer;
using Unity.Game.Spells.Execution;
using UnityEngine;

namespace Unity.Game.Spells
{
    public class SpellController : MonoBehaviour
    {
        public event Action<SpellController> OnSpellComplete;
        
        [SerializeField, HideInInspector] 
        private SpellActivationComponent _activationComponent;
        [SerializeField, HideInInspector]
        private SpellExecutionComponent _executionComponent;
        [SerializeField, HideInInspector]
        private SpellEffectApplier _effectApplier;
        [SerializeField]
        private SpellStateManager _spellState;
        
        private SpellModel _spellModel;

        public SpellActivationComponent ActivationComponent => _activationComponent;

        public SpellExecutionComponent ExecutionComponent => _executionComponent;
        public SpellEffectApplier EffectApplier => _effectApplier;

        public SpellModel Model => _spellModel;



        private void OnValidate()
        {
            _activationComponent = GetComponent<SpellActivationComponent>();
            _executionComponent = GetComponent<SpellExecutionComponent>();
            _effectApplier = GetComponent<SpellEffectApplier>();
            _spellState = GetComponentInChildren<SpellStateManager>();
        }

        public void Initialize(SpellModel spellModedl)
        {
            _spellModel = spellModedl;
        }
            

        public void Dispose()
        {
            OnSpellComplete?.Invoke(this);
            Destroy(gameObject);
        }

        public void Cancel()
        {
            _spellState.Cancel();
        }
    }
}
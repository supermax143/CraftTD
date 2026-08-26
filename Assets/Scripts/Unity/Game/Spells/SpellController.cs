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
        public event Action<SpellController> OnDestroy;
        
        [SerializeField, HideInInspector] 
        private SpellTargetingComponent _targetingComponent;
        [SerializeField, HideInInspector] 
        private SpellActivationComponent _activationComponent;
        [SerializeField, HideInInspector]
        private SpellExecutionComponent _executionComponent;
        [SerializeField, HideInInspector]
        private SpellEffectApplier _effectApplier;
        
        private SpellModel _spellModel;

        public SpellActivationComponent ActivationComponent => _activationComponent;
        public SpellTargetingComponent TargetingComponent => _targetingComponent;

        public SpellExecutionComponent ExecutionComponent => _executionComponent;
        public SpellEffectApplier EffectApplier => _effectApplier;

        public SpellModel Model => _spellModel;



        private void OnValidate()
        {
            _activationComponent = GetComponent<SpellActivationComponent>();
            _targetingComponent = GetComponent<SpellTargetingComponent>();
            _executionComponent = GetComponent<SpellExecutionComponent>();
            _effectApplier = GetComponent<SpellEffectApplier>();
        }

        public void Initialize(SpellModel spellModedl)
        {
            _spellModel = spellModedl;
        }
            

        public void Dispose()
        {
            OnDestroy?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
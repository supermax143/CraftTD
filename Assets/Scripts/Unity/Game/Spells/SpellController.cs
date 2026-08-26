using System;
using Core.Application.Spells.Activation;
using Core.Application.Spells.Targeting;
using Unity.Game.Spells.Execution;
using UnityEngine;

namespace Unity.Game.Spells
{
    public class SpellController : MonoBehaviour
    {
        [SerializeField, HideInInspector] 
        private SpellTargetingComponent _targetingComponent;
        [SerializeField, HideInInspector] 
        private SpellActivationComponent _activationComponent;
        [SerializeField, HideInInspector]
        private SpellExecutionComponent _executionComponent;
        
        public SpellActivationComponent ActivationComponent => _activationComponent;
        public SpellTargetingComponent TargetingComponent => _targetingComponent;

        public SpellExecutionComponent ExecutionComponent => _executionComponent;


        private void OnValidate()
        {
            _activationComponent = GetComponent<SpellActivationComponent>();
            _targetingComponent = GetComponent<SpellTargetingComponent>();
            _executionComponent = GetComponent<SpellExecutionComponent>();
        }

    }
}
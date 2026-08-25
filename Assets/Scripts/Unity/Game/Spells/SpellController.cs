using System;
using Core.Application.Spells.Activation;
using Core.Application.Spells.Targeting;
using UnityEngine;

namespace Unity.Game.Spells
{
    public class SpellController : MonoBehaviour
    {
        [SerializeField, HideInInspector] 
        private SpellTargetingComponent _targetingComponent;
        [SerializeField, HideInInspector] 
        private SpellActivationComponent _activationComponent;
        
        
        public SpellActivationComponent ActivationComponent => _activationComponent;
        public SpellTargetingComponent TargetingComponent => _targetingComponent;


        private void OnValidate()
        {
            _activationComponent = GetComponent<SpellActivationComponent>();
            _targetingComponent = GetComponent<SpellTargetingComponent>();
        }

    }
}
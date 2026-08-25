using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Game.Spells;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    /// <summary>
    /// Менеджер состояний для управления заклинанием
    /// </summary>
      
    [RequireComponent(typeof(TargetingState))]
    [RequireComponent(typeof(ActivationState))]
    [RequireComponent(typeof(CastState))]
    [RequireComponent(typeof(CompleteState))]
    public class SpellStateManager : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private SpellState[] _states;
        [SerializeField]
        private SpellController _spell;
        
        
        private SpellState _currentState;

        private void OnValidate()
        {
            _states = GetComponents<SpellState>();
        }

        private void Start()
        {
            foreach (var state in _states)
            {
                state.Initialize(this, _spell);
                state.enabled = false;
            }
            ChangeState<TargetingState>();
        }
        
        public void ChangeState<T>() where T : SpellState
        {
            if (!TryGetState<T>(out var newState))
            {
                Debug.LogError($"State {typeof(T).Name} not found!");
                return;
            }
            
            if (_currentState != null)
            {
                _currentState.Exit();
                _currentState.enabled = false;
            }

            _currentState = newState;
            _currentState.Enter();
            _currentState.enabled = true;
        }

        public bool TryGetState<T>(out T state) where T : SpellState
        {
            var stateType = typeof(T);
            state = (T)_states.FirstOrDefault(st => st.GetType() == stateType);
            return state != null;
        }

        private void Update()
        {
            _currentState?.UpdateState();
        }
    }
}

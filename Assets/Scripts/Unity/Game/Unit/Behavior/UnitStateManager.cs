using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    /// <summary>
    /// Менеджер состояний для управления поведением юнита
    /// </summary>
    public class UnitStateManager : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private UnitState[] _states;
        
        private UnitController _unit;
        private UnitState _currentState;
        private AttackTarget _currentTarget;

        [Inject] private GameController _gameController;
        
        public AttackTarget CurrentTarget
        {
            get => _currentTarget;
            set => _currentTarget = value;
        }

        public GameController GameController => _gameController;

        private void OnValidate()
        {
            _states = GetComponents<UnitState>();
        }

        public void Initialize(UnitController unit)
        {
            _unit = unit;
            
            var states = GetComponents<UnitState>();
            foreach (var state in states)
            {
                state.Initialize(this, _unit);
            }
        }

        public void ChangeState<T>() where T : UnitState
        {
            //var newStateType = typeof(T);
            
            
            /*if (!_states.ContainsKey(newStateType))
            {
                Debug.LogError($"State {newStateType.Name} not found!");
                return;
            }*/

            // var newState = _states[newStateType];
            
            // var newState = _states.FirstOrDefault(st => st.GetType() == newStateType);
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

        public bool TryGetState<T>(out T state) where T : UnitState
        {
            var stateType = typeof(T);
            state = (T)_states.FirstOrDefault(st => st.GetType() == stateType);
            return state != null;
        }

        private void Update()
        {
            _currentState?.Update();
        }
    }
}

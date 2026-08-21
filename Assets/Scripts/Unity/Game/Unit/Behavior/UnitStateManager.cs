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
      
    [RequireComponent(typeof(WinState))]
    [RequireComponent(typeof(LoseState))]
    [RequireComponent(typeof(SearchTargetState))]
    [RequireComponent(typeof(MoveToTargetState))]
    [RequireComponent(typeof(AttackTargetState))]
    [RequireComponent(typeof(DeathState))]
    public class UnitStateManager : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private UnitState[] _states;
        
        private UnitController _unit;
        private UnitState _currentState;
        private AttackTargetBase _currentTarget;

        [Inject] private IGameController _gameController;
        
        public AttackTargetBase CurrentTarget
        {
            get => _currentTarget;
            set => _currentTarget = value;
        }

        public IGameController GameController => _gameController;

        public UnitState CurrentState => _currentState;

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
                state.enabled = false;
            }
            _unit.HealthComponent.OnDeath += OnUnitDeath;
            _gameController.OnGameFinished += OnGameFinished;
            _gameController.OnDefenseStanceSwitched += OnDefenseStanceSwitched;
        }

        private void OnDefenseStanceSwitched(UnitTier tier, bool active)
        {
            if (_unit.Faction != Faction.Player || _unit.Tier != tier || !active)
            {
                return;
            }
            
            if (_currentState is (DefenseStanceState or AttackTargetState) )
            {
                return;
            }
            
            ChangeState<DefenseStanceState>();
        }


        private void OnUnitDeath()
        {
            
            _unit.HealthComponent.OnDeath -= OnUnitDeath;
            ChangeState<DeathState>();
        }
        
        private void OnGameFinished(Faction winner)
        {
            _gameController.OnGameFinished -= OnGameFinished;
            if (winner == _unit.Faction)
            {
                ChangeState<WinState>();
            }
            else
            {
                ChangeState<LoseState>();
            }
            
        }
        
        public void ChangeState<T>() where T : UnitState
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

        public bool TryGetState<T>(out T state) where T : UnitState
        {
            var stateType = typeof(T);
            state = (T)_states.FirstOrDefault(st => st.GetType() == stateType);
            return state != null;
        }

        private void Update()
        {
            _currentState?.UpdateState();
        }

        private void OnDestroy()
        {
            if (_gameController != null)
            {
                _gameController.OnGameFinished -= OnGameFinished;
                _gameController.OnDefenseStanceSwitched -= OnDefenseStanceSwitched;
            }
            if (_unit != null && _unit.HealthComponent != null)
            {
                _unit.HealthComponent.OnDeath -= OnUnitDeath;
            }
        }
    }
}

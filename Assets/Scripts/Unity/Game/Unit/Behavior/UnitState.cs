using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Базовый класс для всех состояний юнита
    /// </summary>
    public abstract class UnitState : MonoBehaviour
    {
        protected UnitStateManager _stateManager;
        protected UnitController _unit;

        
        
        public virtual void Initialize(UnitStateManager stateManager, UnitController unit)
        {
            _stateManager = stateManager;
            _unit = unit;
            _unit.HealthComponent.OnDeath += OnUnitDeath;
        }

        private void OnUnitDeath()
        {
            _unit.HealthComponent.OnDeath -= OnUnitDeath;
            ChangeState<DeathState>();
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void UpdateState()
        {
        }

        protected void ChangeState<T>() where T : UnitState
        {
            _stateManager.ChangeState<T>();
        }
    }
}

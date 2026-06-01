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
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Update()
        {
        }

        protected void ChangeState<T>() where T : UnitState
        {
            _stateManager.ChangeState<T>();
        }
    }
}

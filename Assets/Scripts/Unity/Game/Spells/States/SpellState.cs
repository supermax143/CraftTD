using Unity.Game.Spells;
using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Базовый класс для всех состояний заклинания
    /// </summary>
    public abstract class SpellState : MonoBehaviour
    {
        protected SpellStateManager _stateManager;
        protected SpellController _spell;

        public virtual void Initialize(SpellStateManager stateManager, SpellController spell)
        {
            _stateManager = stateManager;
            _spell = spell;
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

        protected void ChangeState<T>() where T : SpellState
        {
            _stateManager.ChangeState<T>();
        }
    }
}

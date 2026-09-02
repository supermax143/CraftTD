using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние каста заклинания
    /// </summary>
    public class ExecutionState : SpellState
    {
        public override void Enter()
        {
            _spell.ExecutionComponent.OnComplete += OnSpellExecuted;
            _spell.ExecutionComponent.Execute(_spell);
        }

        private void OnSpellExecuted()
        {
            _spell.ExecutionComponent.OnComplete -= OnSpellExecuted;
            _stateManager.ChangeState<CompleteState>();
        }

        public override void Cancel()//уже нельзя удалить
        {
        }
    }
}

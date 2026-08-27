using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние активации заклинания
    /// </summary>
    public class ActivationState : SpellState
    {
        public override void Enter()
        {
            _spell.ActivationComponent.StartActivationCheck();
        }

        public override void UpdateState()
        {
            if (!_spell.ActivationComponent.IsActivationCheckActive)
            {
                _spell.ActivationComponent.StopActivationCheck();
                _stateManager.ChangeState<ExecutionState>();
            }
        }

       
    }
}

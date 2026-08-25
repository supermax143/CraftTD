using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние выбора цели для заклинания
    /// </summary>
    public class TargetingState : SpellState
    {
        public override void Enter()
        {
            _spell.TargetingComponent.Activate();
        }

        public override void UpdateState()
        {
            if (!_spell.TargetingComponent.IsTargetingActive())
            {
                _stateManager.ChangeState<ActivationState>();
            }
        }

        public override void Exit()
        {
            _spell.TargetingComponent.Deactivate();
        }
    }
}

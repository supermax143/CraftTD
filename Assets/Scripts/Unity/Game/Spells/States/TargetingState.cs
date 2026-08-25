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
            
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

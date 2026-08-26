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
            _spell.ExecutionComponent.Execute(_spell);
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

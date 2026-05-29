using Core.Application.ApplicationSession.States;
using UnityEngine;

namespace Core.Application.ApplicationStateMachine.States
{
    internal class DebugState : SessionStateBase
    {
        protected override void OnStateEnter()
        {
            Debug.Log("Debug State");
        }

        public override void StartGame()
        {
            ApplicationStateMachine.ChangeState<GameState>();
        }
    }
}
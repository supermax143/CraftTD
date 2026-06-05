using Core.Application.ApplicationSession.States;
using Shared.Constants;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Core.Application.ApplicationStateMachine.States
{
    internal class DebugState : SessionStateBase
    {
        protected override void OnStateEnter()
        {
            Debug.Log("Debug State");
            if (_scenesLoader.CurScene == SceneNames.InitGameScene)
            {
                ApplicationStateMachine.ChangeState<MainMenuState>();
            }
        }

        public override void StartGame()
        {
            ApplicationStateMachine.ChangeState<GameState>();
        }
        
        public override void ExitGame()
        {
            ApplicationStateMachine.ChangeState<MainMenuState>();
        }
    }
}
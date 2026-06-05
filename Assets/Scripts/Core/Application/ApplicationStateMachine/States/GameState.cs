using Core.Application.Interfaces;
using Core.Domain.Services;
using Zenject;

namespace Core.Application.ApplicationSession.States {
	internal class GameState : SessionStateBase
	{
		
		
		protected override void OnStateEnter()
		{
			_scenesLoader.LoadGameScene();
		}

		public override void ExitGame()
		{
			ApplicationStateMachine.ChangeState<MainMenuState>();
		}
	}
}
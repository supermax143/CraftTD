using Core.Application.Interfaces;
using Core.Domain.Services;
using Zenject;

namespace Core.Application.ApplicationSession.States {
	internal class GameState : SessionStateBase
	{
		
		[Inject] IScenesLoader _scenesLoader;
		
		protected override void OnStateEnter()
		{
			_scenesLoader.LoadGameScene();
		}
	}
}
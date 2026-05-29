using Core.Application.Interfaces;
using Core.Domain.Services;
using Shared.Constants;
using Zenject;

namespace Core.Application.ApplicationSession.States
{
   internal class BootstrapState : SessionStateBase
   {
      [Inject] private IScenesLoader _scenesLoader;
      [Inject] private IGameBootstrap _gameBootstrap;
      
      protected override async void OnStateEnter()
      {
         if (_scenesLoader.CurScene != SceneNames.InitGameScene)
         {
            await _scenesLoader.LoadInitGameScene();
         }
         await _gameBootstrap.Initialize();
         ApplicationStateMachine.ChangeState<MainMenuState>();
      }
      
   }
}
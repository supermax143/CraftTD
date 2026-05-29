using Core.Application.Interfaces;
using Core.Domain.Services;
using Shared.Constants;
using Zenject;

namespace Core.Application.ApplicationSession.States
{
   internal class MainMenuState : SessionStateBase
   {

      [Inject] IScenesLoader _scenesLoader;
      
      protected override void OnStateEnter()
      {
         if (_scenesLoader.CurScene == SceneNames.MainMenuScene)
         {
            return;
         }
         
         _scenesLoader.LoadMainMenuScene();
      }

      public override void StartGame() 
      {
         ApplicationStateMachine.ChangeState<GameState>();
      }

   }
}
using System.Threading.Tasks;
using Core.Application.Interfaces;
using Core.Domain.Services;
using Shared.Constants;
using Unity.Infrastructure.GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Unity.Infrastructure.Scenes
{
   internal class ScenesLoader : IScenesLoader  
   {
      [Inject] private ZenjectSceneLoader _sceneLoader;
      [Inject] private GameEventsBus _eventsBus;


      public string CurScene => SceneManager.GetActiveScene().name;


      public async Task  LoadInitGameScene() => await LoadScene(SceneNames.InitGameScene);
      public async Task  LoadMainMenuScene() => await LoadScene(SceneNames.MainMenuScene);
      public async Task  LoadGameScene() => await LoadScene(SceneNames.GameScene);

      private async Task LoadScene(string scene)
      {
         if (CurScene == scene)
         {
            return;
         }
         await _sceneLoader.LoadSceneAsync(scene);
         _eventsBus.TriggerEvent(new SceneEvent(SceneEvent.Type.Opened, scene));
      }
      

   }
}
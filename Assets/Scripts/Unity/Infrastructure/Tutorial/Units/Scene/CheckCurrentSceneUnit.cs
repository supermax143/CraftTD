using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

namespace Unity.Infrastructure.Tutorial.Units.Scene
{
    [UnitCategory("Custom/Scene")]
    [UnitTitle("Check Current Scene")]
    public class CheckCurrentSceneUnit : CustomGetUnit<bool>
    {
        private const string SCENE_NAME = "sceneName";
        
        [DoNotSerialize] private ValueInput _sceneName;

        
        protected override void Definition()
        {
            base.Definition();
            _sceneName = ValueInput(SCENE_NAME, "");
        }


        protected override bool GetResult(Flow flow)
        {
            
            var expectedSceneName = flow.GetValue<string>(_sceneName);
            
            if (string.IsNullOrWhiteSpace(expectedSceneName))
            {
                return false;
            }
            
            var currentScene = SceneManager.GetActiveScene();
            return currentScene.name == expectedSceneName;
        }
        
    }
}

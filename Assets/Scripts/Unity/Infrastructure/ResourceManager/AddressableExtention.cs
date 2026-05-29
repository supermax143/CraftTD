using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Unity.Infrastructure.ResourceManager
{
    public static class AddressableExtention
    {
        private static HandleStorage _handleStorage;


        public static void Initialize(HandleStorage handleStorage)
        {
            _handleStorage = handleStorage;
        }
        
        public static Task<T> Load<T>(string key, GameObject link)
            where T: Object => Addressables.LoadAssetAsync<T>(key).LoadAndRegisterHandle(link);

        public static Task<T> Load<T>(string key, string tag)
            where T: Object => Addressables.LoadAssetAsync<T>(key).LoadAndRegisterHandle(tag);

        public static Task<SceneInstance> LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single) 
            => Addressables.LoadSceneAsync(sceneName, loadMode).Task;
        
        public static Task UnloadScene(SceneInstance sceneInstance) 
            => Addressables.UnloadSceneAsync(sceneInstance).Task;
        
        public static bool HasEntry<T>(string key)
            => Addressables.ResourceLocators.Any(l => l.Locate(key, typeof(T), out _));

        public static void ReleaseTag(string tag) => _handleStorage.ReleaseTag(tag);
        
        public static Task<IList<T>> LoadByLabel<T>(string label, GameObject gameObject)
            where T: Object => Addressables.LoadAssetsAsync<T>(label, delegate { }).LoadAndRegisterHandle(gameObject);
        
        public static Task<IList<T>> LoadByLabel<T>(string label, string tag)
            where T: Object => Addressables.LoadAssetsAsync<T>(label, delegate { }).LoadAndRegisterHandle(tag);
    }
}
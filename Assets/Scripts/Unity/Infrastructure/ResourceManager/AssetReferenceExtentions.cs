using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unity.Infrastructure.ResourceManager
{
    public static class AssetReferenceExtension
    {
        public static async Task<T> LoadAssetReference<T>(this AssetReference reference, HandleDependencies dependencies)
            where T: Object
        {
            var loadTask = TryGetLoadTask<T>(reference);

            if (loadTask is not null)
                return (T)await loadTask;

            return await reference.LoadAssetAsync<T>().LoadAndRegisterHandle(dependencies);
        }

        public static async Task<T> LoadAssetReference<T>(this AssetReference reference, GameObject link)
            where T: Object
        {
            var loadTask = TryGetLoadTask<T>(reference);

            if (loadTask is not null)
                return (T)await loadTask;

            return await reference.LoadAssetAsync<T>().LoadAndRegisterHandle(link);
        }

        public static async Task<T> LoadAssetReference<T>(this AssetReference reference, string tag)
            where T: Object
        {
            var loadTask = TryGetLoadTask<T>(reference);

            if (loadTask is not null)
                return (T)await loadTask;

            return await reference.LoadAssetAsync<T>().LoadAndRegisterHandle(tag);
        }

        private static Task<object> TryGetLoadTask<T>(this AssetReference reference)
            where T: Object
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return Task.FromResult((object)reference.editorAsset);
#endif

            return !reference.IsValid() ? null : reference.OperationHandle.Task;
        }

        public static Task<T> LoadAssetReference<T>(this AssetReferenceT<T> reference, HandleDependencies dependencies)
            where T: Object => LoadAssetReference<T>((AssetReference)reference, dependencies);

        public static Task<T> LoadAssetReference<T>(this AssetReferenceT<T> reference, string tag)
            where T: Object => LoadAssetReference<T>((AssetReference)reference, tag);

        public static Task<T> LoadAssetReference<T>(this AssetReferenceT<T> reference, GameObject link)
            where T: Object => LoadAssetReference<T>((AssetReference)reference, link);

        public static Task LoadScene(string envPath) => Addressables.LoadSceneAsync(envPath).Task;
    }
}
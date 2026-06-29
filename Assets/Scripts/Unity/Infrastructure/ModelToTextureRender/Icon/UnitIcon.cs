using Cysharp.Threading.Tasks;
using Exploration.Scripts.Controllers.ModelRender;
using Unity.Infrastructure.ResourceManager;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Environments.Land.Scripts.Runtime.GUI
{
    public class UnitIcon : IconBase
    {

        private AssetReference _assetReference;
        
        protected override async UniTask SpawnLandObject(AssetReference assetReference)
        {
           _assetReference = assetReference;
            var prototype = await _assetReference.LoadAssetReference<Object>(_assetReference.AssetGUID);
            var targetGO = _diContainer.InstantiatePrefab(prototype);
            _modelHolder = _diContainer.InstantiateComponent<ModelHolder>(targetGO);
            _modelHolder.AddRenderTarget(_rendererTarget);
            _rendererTarget.SetWorldSize(_modelHolder.GetWorldBounds().size);
        }

        public override void Dispose()
        {
            AddressableExtention.ReleaseTag(_assetReference.AssetGUID);
            if (_modelHolder != null && _modelHolder.gameObject != null)
            {
                Destroy(_modelHolder.gameObject);
            }
            base.Dispose();
        }
    }
}
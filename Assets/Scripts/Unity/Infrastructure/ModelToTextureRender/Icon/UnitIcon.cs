using Cysharp.Threading.Tasks;
using Exploration.Scripts.Controllers.ModelRender;
using Unity.Game;
using Unity.Infrastructure.ResourceManager;
using Unity.VisualScripting;
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
            var unitPrefab = await _assetReference.LoadAssetReference<Object>(_assetReference.AssetGUID);
            var prototype = unitPrefab.GetComponentInChildren<UnitView>().gameObject;
            
            var targetGO = Instantiate(prototype);
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
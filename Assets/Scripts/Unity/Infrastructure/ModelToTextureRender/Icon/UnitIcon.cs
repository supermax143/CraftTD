using Cysharp.Threading.Tasks;
using Exploration.Scripts.Controllers.ModelRender;
using Unity.Game;
using Unity.Infrastructure.ResourceManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Environments.Land.Scripts.Runtime.GUI
{
    public class UnitIcon : IconBase
    {

        private AssetReference _assetReference;
        
        public RawImage Image => _rendererTarget.Image;
        
        public UnitView UnitView => _modelHolder.GetComponent<UnitView>();
        
        protected override async UniTask SpawnLandObject(AssetReference assetReference)
        {
            if (_modelHolder != null)
            {
                Dispose();
            }
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
            if (_assetReference != null)
            {
                AddressableExtention.ReleaseTag(_assetReference.AssetGUID);
            }
            
            base.Dispose();
        }
    }
}
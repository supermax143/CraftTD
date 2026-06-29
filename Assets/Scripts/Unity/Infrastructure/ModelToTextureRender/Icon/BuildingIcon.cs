using Cysharp.Threading.Tasks;
using Exploration.Scripts.Controllers.ModelRender;
using UnityEngine;

namespace Environments.Land.Scripts.Runtime.GUI
{
    public class BuildingIcon : LandObjectIconBase
    {

        public GameObject UnitPrefab;
        
        
        protected override async UniTask SpawnLandObject(ulong prototypeId)
        {
            /* (!InteractiveMap.TryGetName(prototypeId, out var key))
            {
                this.LogError($"Can't find related interactive name for id {prototypeId}");
                return;
            }

            if (!AddressableExtention.HasEntry<GameObject>(key))
            {
                key = "fallback_lo";
            }*/

            var prototype = UnitPrefab;//await AddressableExtention.Load<GameObject>(key, _anchorGameObject);
            var landObjectGO = _diContainer.InstantiatePrefab(prototype);
            _modelHolder = _diContainer.InstantiateComponent<ModelHolder>(landObjectGO);
            _modelHolder.AddRenderTarget(_rendererTarget);
            _rendererTarget.SetWorldSize(_modelHolder.GetWorldBounds().size);
        }

        public override void Dispose()
        {
            if (_modelHolder != null && _modelHolder.gameObject != null)
            {
                Destroy(_modelHolder.gameObject);
            }
            base.Dispose();
        }
    }
}
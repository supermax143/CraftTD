using Cysharp.Threading.Tasks;
using Environments.Land.Scripts.Runtime.GUI;
using Exploration.Scripts.Controllers.ModelRender;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

[RequireComponent(typeof(ImageRendererTarget))]
public abstract class IconBase : MonoBehaviour
{
    
    [Inject] protected DiContainer _diContainer;
    
    [SerializeField, HideInInspector]
    protected ImageRendererTarget _rendererTarget;

    protected ModelHolder _modelHolder;

    protected GameObject _anchorGameObject;
    
    private void OnValidate()
    {
        _rendererTarget = GetComponent<ImageRendererTarget>();
    }

    public async UniTask Initialize(AssetReference assetReference, GameObject anchorGO = null)
    {
        _anchorGameObject = anchorGO != null ? anchorGO : gameObject;
        await SpawnLandObject(assetReference);
        _rendererTarget.RebuildUI();
    }

    public Transform GetTransform() => transform;

    protected abstract UniTask SpawnLandObject(AssetReference assetReference);

    private void OnDestroy()
    {
        Dispose();
    }

    public virtual void Dispose()
    {
        if (_modelHolder != null)
        {
            _modelHolder.Dispose();
        }
    }
}

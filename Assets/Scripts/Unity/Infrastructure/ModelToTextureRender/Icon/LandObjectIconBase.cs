using Cysharp.Threading.Tasks;
using Environments.Land.Scripts.Runtime.GUI;
using Exploration.Scripts.Controllers.ModelRender;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ImageRendererTarget))]
public abstract class LandObjectIconBase : MonoBehaviour
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

    public async UniTask Initialize(ulong prototypeId, GameObject anchorGO = null)
    {

        _anchorGameObject = anchorGO != null ? anchorGO : gameObject;
        await SpawnLandObject(prototypeId);
        _rendererTarget.RebuildUI();
    }

    public Transform GetTransform() => transform;

    protected abstract UniTask SpawnLandObject(ulong prototypeId);

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

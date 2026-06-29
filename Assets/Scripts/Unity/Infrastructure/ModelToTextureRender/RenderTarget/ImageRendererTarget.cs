using Exploration.Scripts.Controllers.ModelRender;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ImageRendererTarget : RenderTargetBase
{

    [SerializeField, HideInInspector]
    private RawImage _image;

    private Vector2 _startSize;
    private Vector2 _renderSize;
    
    private void OnValidate()
    {
        _image = GetComponent<RawImage>();
    }
    
    private void Awake()
    {
        _image.material = Instantiate(_image.material);
        _startSize = Bounds.size;
    }


    public override Bounds Bounds => new()
    {
        min = _image.rectTransform.rect.min,
        max = _image.rectTransform.rect.max
    };

    public void SetWorldSize(Vector2 value)
    {
        _renderSize = value;
        PreserveAspect();
    }

    public override void SetMaterial(Material mat)
    {
        _image.material.mainTexture = mat.mainTexture;
    }

    public override void SetUV(Vector2 start, Vector2 size)
    {
        _image.uvRect = new Rect(start, size);
        PreserveAspect();
    }

    public void PreserveAspect()
    {
        var newSize = _startSize;
        if (_renderSize.x > _renderSize.y)
        {
            newSize.y = (newSize.y * _renderSize.y) / _renderSize.x;
        }
        else if(_renderSize.y > _renderSize.x)
        {
            newSize.x = (newSize.x * _renderSize.x) / _renderSize.y;
        }
        _image.rectTransform.sizeDelta = newSize;
        RebuildUI();
    }

    public void RebuildUI() 
    {
#if UNITY_EDITOR
        _image.OnRebuildRequested();
#endif
    }
}

using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Компонент для управления тинтингом материала через MaterialPropertyBlock
/// Позволяет применять тинт только к конкретному рендереру без изменения материала
/// </summary>
public class TintController : MonoBehaviour
{
    [SerializeField] 
    protected Color _tintColor = new Color(1, 1, 1, 0);
    [SerializeField]
    private Renderer[] _targetRenderers;
    [SerializeField]
    private string _tintColorProperty;
    
    private int _tintColorID;
    
    private MaterialPropertyBlock _propertyBlock;
    
    protected Color _lastTintColor;
    
    
    private void Awake()
    {
        _tintColorID = Shader.PropertyToID(_tintColorProperty);
        if (_targetRenderers == null || _targetRenderers.Length == 0)
        {
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                _targetRenderers = new Renderer[] { renderer };
            }
        }
        
        if (_targetRenderers == null || _targetRenderers.Length == 0)
        {
            Debug.LogError($"TintController: No renderers found on {gameObject.name}", this);
            enabled = false;
            return;
        }
        
        _propertyBlock = new MaterialPropertyBlock();
        _lastTintColor = _tintColor;
        ApplyTint();
    }

    public void SetTintColor(Color color)
    {
        if (color == _tintColor)
        {
            return;
        }
        _tintColor = color;
        Debug.Log($"SetTintColor:{color}");
        ApplyTint();
    }
    
    protected void ApplyTint()
    {
        if (_targetRenderers == null ||
            _targetRenderers.Length == 0 ||
            _propertyBlock == null)
        {
            return;
        }
        
        _propertyBlock.SetColor(_tintColorID, _tintColor);
        
        foreach (var renderer in _targetRenderers)
        {
            if (renderer != null)
            {
                renderer.SetPropertyBlock(_propertyBlock);
            }
        }
    }
    
}

using System;
using UnityEngine;

namespace Exploration.Scripts.Controllers.ModelRender
{
    /// <summary>
    /// тут отображаем отрендеренную из модели текстуру
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class QuadRenderTarget : RenderTargetBase
    {
        [SerializeField, HideInInspector] 
        private Renderer _renderer = null;

        [SerializeField, HideInInspector]
        private MeshFilter _meshFilter = null;
    
        public override Bounds Bounds => _renderer.bounds;
        
        protected void OnValidate()
        {
            _meshFilter = GetComponent<MeshFilter>();
            if (_meshFilter == null)
            {
                throw new InvalidOperationException("mesh filter not added");
            }
            _renderer = GetComponent<Renderer>();
        }
      
        public override void SetTexture(Texture texture)
        {
            _renderer.sharedMaterial.mainTexture = texture;
        }
        
        public override void SetUV(Vector2 start, Vector2 size)
        {
            var uvs = _meshFilter.mesh.uv;
            var rect = new Rect(start, size);
            uvs[0] = rect.min;
            uvs[1] = new Vector2(rect.xMax, rect.yMin);
            uvs[2] = new Vector2(rect.xMin, rect.yMax);
            uvs[3] = rect.max;
            
            _meshFilter.mesh.uv = uvs;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Utils;
using UnityEngine;
using Zenject;

namespace Exploration.Scripts.Controllers.ModelRender
{

    /// <summary>
    /// прослойка между моделью и рендером
    /// </summary>
    public class ModelHolder : MonoBehaviour, IDisposable
    {
        
        private const float TARGET_TEXTURE_PPU = 128;
        private const float TARGET_RENDER_SIZE = 2 * 2.5f;//за пример взят размер квада для нпц
        
        [SerializeField] 
        private RenderTargetBase _renderTarget = null;
        [SerializeField]
        private bool _boundsFromRenderTarget = false;
        [SerializeField]
        private bool _boundsFromModel = true;
        [SerializeField]
        private bool _checkCamera = false;
        
        [Inject] private readonly IModelToTextureRenderer _modelToTextureRenderer = null;
        
        public IEnumerable<Renderer> Renderers => _renderers;
        public Transform ModelTransform => transform;
        public RenderTargetBase RenderTarget => _renderTarget;
        private readonly List<Renderer> _renderers = new();

        private Bounds _bounds;
        
        
        public float TexturePPU { get; private set; } = TARGET_TEXTURE_PPU;

        private void UpdateTexturePPU()
        {
            var size = GetWorldBounds().size;
            var scale = Math.Clamp(TARGET_RENDER_SIZE / (size.x * size.y), .4f, 1);
            TexturePPU = TARGET_TEXTURE_PPU * scale;
        }

        public Bounds GetWorldBounds()
        {
            if (_boundsFromModel)
            {
                return GetComponentInChildren<CharBounds>().GetBounds();
            }
            else if(_boundsFromRenderTarget)
            {
                return _renderTarget.Bounds;
            }

            return _bounds;
        }

        public Vector2 GetRenderSize() => GetWorldBounds().size * TexturePPU;
        
        public int RectId { get; set; }

        public bool CheckCamera => _checkCamera;

        public void Initialize()
        {
            _renderers.Clear();
            AddRenderers(GetComponentsInChildren<Renderer>(true));
            UpdateTexturePPU();
            if (_renderTarget != null)
            {
                _modelToTextureRenderer.AddModelHolder(this);
            }

            if (_boundsFromModel)
            {
                _bounds = GetComponentInChildren<CharBounds>().GetBounds();
            }
            else if(_boundsFromRenderTarget)
            {
                _bounds = _renderTarget.Bounds;
            }
        }

        public void AddRenderTarget(RenderTargetBase renderTarget)
        {
            if (_renderTarget != null)
            {
                return;
            }
            
            _renderTarget = renderTarget;
            Initialize();
        }

        
        public void AddRenderers(Renderer[] renderers)
        {
            var sortedRenderers = 
                renderers.
                    OrderByDescending(r => r.transform.position.z).
                    ThenBy(r => r.sortingOrder);

            foreach (var r in sortedRenderers)
            {
                AddRenderer(r);
            }
        }

        public void AddRenderer(Renderer renderer)
        {
            if (_renderers.Contains(renderer))
            {
                return;
            }
            
            AddRendererBounds(renderer);
            
            renderer.gameObject.layer = Layers.Hidden;
            _renderers.Add(renderer);
        }

        public void SetTexture(Texture texture)
        {
            _renderers.ForEach(r => r.material.SetTexture("_MainTex", texture));
        }
        
        private void AddRendererBounds(Renderer renderer)
        {
            if (_boundsFromModel || _boundsFromRenderTarget)
            {
                return;
            }
            
            if (_bounds == default)
            {
                _bounds = renderer.bounds;
            }
            else
            {
                _bounds.Encapsulate(renderer.bounds);
            }
        }
        
        
        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_modelToTextureRenderer != null)
            {
                _modelToTextureRenderer.RemoveModelHolder(this);
            }

            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
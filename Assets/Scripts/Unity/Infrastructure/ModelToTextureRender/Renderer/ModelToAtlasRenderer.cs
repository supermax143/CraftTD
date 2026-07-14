using System.Collections.Generic;
using Environments.Land.Scripts.Runtime.GUI;
using Exploration.Scripts.Controllers.ModelToTextureRender.RectanglePacking;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D.Animation;
using Zenject;

namespace Exploration.Scripts.Controllers.ModelRender
{
    
    /// <summary>
    /// рендерит в атлас 
    /// </summary>
    public class ModelToAtlasRenderer : ModelToTextureRendererBase, IModelToTextureRenderer, IRenderModelsSpawnHandler
    {
        private class AtlasPage
        {
            public RenderTexture AtlasTexture { get; private set; }
            public RenderTargetIdentifier RenderTargetIdentifier { get; private set; }
            public int Index { get; private set; }
            public Material Material { get; private set; }
            
            public AtlasPage(Material material,RenderTexture texture, RenderTargetIdentifier rti, int index)
            {
                AtlasTexture = texture;
                RenderTargetIdentifier = rti;
                Index = index;
                Material = material;
            }
        }
        
        private static class Properties
        {
            public static int MainTexture = Shader.PropertyToID("_MainTex");
        }
        
        private const int ATLAS_PAGE_WIDTH = 2048;
        private const int ATLAS_PAGE_HEIGHT = 1024;
        
        [SerializeField]
        private Material _material = null;

        
        private readonly List<ModelHolder> _modelHolders = new();
        private Camera Camera => Camera.main;

        private readonly List<AtlasPage> _atlasPages = new();

        private RectanglePacker _packer;
        private readonly Dictionary<int, ModelHolder> _modelHoldersByRectId = new();

        private bool _atlasPackBlocked = false;

        public void BlockAtlasPack()
        {
            _atlasPackBlocked = true;
        }
        
        public void UnblockAtlasPack()
        {
            _atlasPackBlocked = false;
            PackAtlas();
        }
        
        public void AddModelHolder(ModelHolder modelHolder)
        {
            if (_modelHolders.Contains(modelHolder))
            {
                return;
            }
            _modelHolders.Add(modelHolder);
            Init();
        }

        public void RemoveModelHolder(ModelHolder modelHolder)
        {
            if (!_modelHolders.Contains(modelHolder))
            {
                return;
            }

            _modelHolders.Remove(modelHolder);
            PackAtlas();
        }
        
        protected override void InitTexture()
        {
            PackAtlas();
        }
       
        private void PackAtlas()
        {
            if (gameObject == null)
            {
                return;
            }
            
            if (_atlasPackBlocked)
            {
                return;
            }
            _modelHoldersByRectId.Clear();
            if (_packer == null)
            {
                _packer = new RectanglePacker(ATLAS_PAGE_WIDTH, ATLAS_PAGE_HEIGHT, 1);
            }
            else
            {
                _packer.Reset(ATLAS_PAGE_WIDTH, ATLAS_PAGE_HEIGHT, 1);
            }
            
            foreach (var holder in _modelHolders)
            {
                var size = holder.GetRenderSize();
                var rectId = _packer.AddRectangle((int)size.x, (int)size.y);
                holder.RectId = rectId;
                _modelHoldersByRectId[rectId] = holder;
            }
            
            _packer.PackRectangles(SortRects);
            
            foreach (var holder in _modelHolders)
            {
                _packer.TryGetRectangle(holder.RectId, out var rect);
                var pageIndex = rect.PageIndex;
                var atlasPage = GetOrCreateAtlasPage(pageIndex);
                
                holder.RenderTarget.SetMaterial(atlasPage.Material);
                
                var pos = new Vector2(rect.X, rect.Y);
                pos.x /= ATLAS_PAGE_WIDTH;
                pos.y /= ATLAS_PAGE_HEIGHT;
                var size = new Vector2(rect.Width, rect.Height);
                size.x /= ATLAS_PAGE_WIDTH;
                size.y /= ATLAS_PAGE_HEIGHT;
                holder.RenderTarget.SetUV(pos, size);
            }
        }

        //сортируем по дальности от камеры, для группировки страницы атласа
        private void SortRects(List<SortableSize> rectangles)
        {
            rectangles.Sort(SortDistance);
        }

        private int SortDistance(SortableSize x, SortableSize y)
        {
            var distX = GetDistanceToCamera(x);
            var distY = GetDistanceToCamera(y);
            return distY.CompareTo(distX);
        }

        private float GetDistanceToCamera(SortableSize rect)
        {
            if (!_modelHoldersByRectId.TryGetValue(rect.Id, out var holder) || Camera == null)
            {
                return 0;
            }
            var bounds = holder.GetWorldBounds();
            var distance = Vector3.Magnitude(Camera.WorldToScreenPoint(bounds.center) - Camera.WorldToScreenPoint(Camera.transform.position));
            return distance;
        }

        private AtlasPage GetOrCreateAtlasPage(int index)
        {
            AtlasPage page;
            if (index >= _atlasPages.Count)
            {
                var renderTarget = CreateRenderTarget(ATLAS_PAGE_WIDTH, ATLAS_PAGE_HEIGHT);
                var pageMaterial = Instantiate(_material);
                pageMaterial.SetTexture(Properties.MainTexture, renderTarget.texture);
                page = new AtlasPage(pageMaterial, renderTarget.texture, renderTarget.rti, index);
                _atlasPages.Add(page);
            }
            else
            {
                page = _atlasPages[index];
            }

            return page;
        }
        
        
        protected override void Render()
        {
            if (_atlasPages.Count == 0)
            {
                return;
            }

            RenderModels();
        }

        private void RenderModels()
        {
            var modelsRendered = 0;
            var screenBounds = new Rect(0, 0, Screen.width, Screen.height);
            var needRepack = false;
            for (int pageIndex = 0; pageIndex < _packer.PagesCount; pageIndex++)
            {
                CommandBuffer.Clear();
                var rects = _packer.GetPageRects(pageIndex);
                var atlasPage = GetOrCreateAtlasPage(pageIndex);
                
                CommandBuffer.SetRenderTarget(atlasPage.RenderTargetIdentifier);
                CommandBuffer.ClearRenderTarget(clearDepth: true, clearColor: true, Color.clear);
                
                foreach (var rect in rects)
                {
                    if (!_modelHoldersByRectId.TryGetValue(rect.ID, out var holder))
                    {
                        continue;
                    }

                    if (holder == null)
                    {
                        _modelHoldersByRectId.Remove(rect.ID);
                        _modelHolders.Remove(holder);
                        needRepack = true;
                        continue;
                    }

                    var bounds = holder.GetWorldBounds();
                    if (!IsBoundsInsideScreen(screenBounds, bounds) && holder.CheckCamera)
                    {
                        continue;
                    }
                    else if(pageIndex > 0 && holder.CheckCamera)
                    {
                        needRepack = true;
                    }
                    
                    CommandBuffer.SetViewProjectionMatrices(GetView(holder), GetProjection(holder, rect));
                    
                    foreach (var rnd in holder.Renderers)
                    {
                        if (!IsRendererValid(rnd) || rnd.sharedMaterial == null)
                        {
                            continue;
                        }

                        /*var skin = rnd.GetComponent<SpriteSkin>();
                        if (skin != null)
                        {
                            skin.forceCpuDeformation = true;
                            skin.alwaysUpdate = true;
                            skin.autoRebind = true;
                        }*/
                        CommandBuffer.DrawRenderer(rnd, rnd.sharedMaterial);
                    }
        
                    modelsRendered++;
                }

                if (modelsRendered > 0)
                {
                    Graphics.ExecuteCommandBuffer(CommandBuffer);
                    modelsRendered = 0;
                }
            }

            if (needRepack)
            {
                PackAtlas();
            }
        }

        protected Matrix4x4 GetProjection(ModelHolder modelHolder, IntegerRectangle rectangle)
        {
            var renderSize = modelHolder.GetRenderSize();
            Vector2 scale;
            scale.x = ATLAS_PAGE_WIDTH / renderSize.x;
            scale.y = ATLAS_PAGE_HEIGHT / renderSize.y;
            
            var worldBounds = modelHolder.GetWorldBounds();

            var bounds = new Bounds();
            bounds.size = worldBounds.size * scale;
            
            bounds.center = worldBounds.center;
            var moveToStartDelta = worldBounds.size * (scale * 0.5f - new Vector2(.5f,.5f));
            bounds.center += (Vector3)moveToStartDelta;
            bounds.center -= new Vector3(rectangle.X, rectangle.Y, 0) / modelHolder.TexturePPU;
            
            return Matrix4x4.Ortho(bounds.min.x, bounds.max.x, bounds.min.y , bounds.max.y, Z_NEAR_PLANE, Z_FAR_PLANE);
        }

        private bool IsBoundsInsideScreen(Rect screenRect, Bounds bounds)
        {
            var cam = Camera;
            var testRect = new Rect()
            {
                min = cam.WorldToScreenPoint(bounds.min),
                max = cam.WorldToScreenPoint(bounds.max)
            };

            return screenRect.Overlaps(testRect);
        }

        protected override void OnDestroy()
        {
            foreach (var page in _atlasPages)
            {
                DisposeTexture(page.AtlasTexture);
            }
            _atlasPages.Clear();
            _modelHolders.Clear();
            _modelHoldersByRectId.Clear();
        }

        public void OnModelsStartSpawn()
        {
            BlockAtlasPack();
        }

        public void OnModelsEndSpawn()
        {
            UnblockAtlasPack();
        }

        public void OnModelsStartDestroy()
        {
            BlockAtlasPack();
        }

        public void OnModelsEndDestroy()
        {
            UnblockAtlasPack();
        }
    }
}
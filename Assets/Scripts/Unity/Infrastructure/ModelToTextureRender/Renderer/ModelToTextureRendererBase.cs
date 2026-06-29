using Unity.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace Exploration.Scripts.Controllers.ModelRender
{
    
    /// <summary>
    /// базовый класс рендеринга модели в квад
    /// </summary>
    public abstract class ModelToTextureRendererBase : MonoBehaviour
    {
        protected const int Z_NEAR_PLANE = -10;
        protected const int Z_FAR_PLANE = 50;
        private const int ANTI_ALIASING = 4;

        protected CommandBuffer CommandBuffer { get; private set; } = null;

        protected abstract void Render();
        protected abstract void InitTexture();
        protected abstract void OnDestroy();
        
       
        
        protected void Init()
        {
            CommandBuffer = new CommandBuffer();
            CommandBuffer.name = "ModelsToTexture";
            InitTexture();
        }

        protected (RenderTexture texture, RenderTargetIdentifier rti) CreateRenderTarget(int width, int height)
        {
            var renderTexture = RenderTexture.GetTemporary(width
                , height
                , depthBuffer: 8
                , RenderTextureFormat.ARGB32);
            renderTexture.antiAliasing = ANTI_ALIASING;
            var rti = new RenderTargetIdentifier(renderTexture);
            return (renderTexture, rti);
        }
        
        
        protected Matrix4x4 GetView(ModelHolder modelHolder)
        {
            if (!modelHolder || !modelHolder.ModelTransform)
            {
                return Matrix4x4.identity;
            }
         
            var matrix = Matrix4x4.identity;
            var modelPos = modelHolder.ModelTransform.position;
         
            matrix.SetTRS(new Vector3(0, 0, modelPos.z) , Quaternion.identity, new Vector3(1,1,-1));

            return matrix;
        }

        protected Matrix4x4 GetProjection(Bounds bounds, Vector2Int offset, Vector2 scale)
        {
            float posX = bounds.size.x * (scale.x / 2 - .5f);
            float posY = bounds.size.y * (scale.y / 2 - .5f);
            
            Vector2 startPosition = new Vector2(posX, posY);
            bounds.center += (Vector3)startPosition;
            bounds.center -= new Vector3(offset.x * bounds.size.x, offset.y * bounds.size.y, 0);
            
            var size = bounds.size;
            size.x *= scale.x;
            size.y *= scale.y;
            bounds.size = size;
            
            return Matrix4x4.Ortho(bounds.min.x, bounds.max.x, bounds.min.y , bounds.max.y, Z_NEAR_PLANE, Z_FAR_PLANE);
        }


        protected virtual void DisposeTexture(RenderTexture texture)
        {
            if (texture == null)
            {
                return;
            }
            
            texture.Release();
        }

        private void LateUpdate()
        {
            Render();
        }

        protected bool IsRendererValid(Renderer renderer)
        {
            return renderer != null && 
                   renderer.gameObject.activeInHierarchy &&
                   renderer.gameObject.layer == Layers.Hidden;
        }
    }
}
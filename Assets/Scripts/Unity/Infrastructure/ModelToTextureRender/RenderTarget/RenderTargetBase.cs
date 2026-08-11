using UnityEngine;

namespace Exploration.Scripts.Controllers.ModelRender
{
    public abstract class RenderTargetBase : MonoBehaviour
    {
        public abstract Bounds Bounds { get; }


        public abstract void SetTexture(Texture texture);


        public abstract void SetUV(Vector2 start, Vector2 size);
       
    }
}
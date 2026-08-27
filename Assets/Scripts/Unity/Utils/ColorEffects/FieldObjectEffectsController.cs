using System.Collections;
using Unity.Utils.Time;
using UnityEngine;

namespace Utils.ColorEffects
{
    public class FieldObjectEffectsController : MonoBehaviour
    {
        
        [SerializeField, HideInInspector]
        private SpriteRenderer[] _renderers;

        
        public float dissolveDuration = 2;

        private MaterialPropertyBlock _propertyBlock;
        private Bounds? _cachedBounds;

        private static class ShaderProperties
        {
            public static readonly int _Dissolve = Shader.PropertyToID(nameof(_Dissolve));
            public static readonly int _DissolveAmount = Shader.PropertyToID(nameof(_DissolveAmount));
            public static readonly int _VerticalDissolve = Shader.PropertyToID(nameof(_VerticalDissolve));
            public static readonly int _VerticalDissolveAmount = Shader.PropertyToID(nameof(_VerticalDissolveAmount));
            public static readonly int _BoundsTop = Shader.PropertyToID(nameof(_BoundsTop));
            public static readonly int _BoundsBottom = Shader.PropertyToID(nameof(_BoundsBottom));
            public static readonly int _HitEffect = Shader.PropertyToID(nameof(_HitEffect));
            public static readonly int _HitAmount = Shader.PropertyToID(nameof(_HitAmount));
            public static readonly int _HorizontalDissolve = Shader.PropertyToID(nameof(_HorizontalDissolve));
            public static readonly int _HorizontalDissolveAmount = Shader.PropertyToID(nameof(_HorizontalDissolveAmount));
            public static readonly int _BoundsLeft = Shader.PropertyToID(nameof(_BoundsLeft));
            public static readonly int _BoundsRight = Shader.PropertyToID(nameof(_BoundsRight));
            public static readonly int _HorizontalDissolveIversed = Shader.PropertyToID(nameof(_HorizontalDissolveIversed));

        }

        private readonly Timer _hitAnimationTimer = new(TimeType.Scaled);
        private readonly Timer _dissolveAnimationTimer = new(TimeType.Scaled);
        private readonly Timer _verticalDissolveAnimationTimer = new(TimeType.Scaled);
        private readonly Timer _horizontalDissolveAnimationTimer = new(TimeType.Scaled);

        public SpriteRenderer[] Renderers => _renderers;

        private void OnValidate()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
            _cachedBounds = null;
        }
        
        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }
        
        public IEnumerator ShowDissolveEffect(float time)
        {
            _dissolveAnimationTimer.Start(time);
            _propertyBlock.SetFloat(ShaderProperties._Dissolve, 1);
            while (!_dissolveAnimationTimer.IsComplete)
            {
                _propertyBlock.SetFloat(ShaderProperties._DissolveAmount, _dissolveAnimationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            //_propertyBlock.SetFloat(ShaderProperties._Dissolve, 0);
            UpdateRenderersPropertyBlock();
        }
        
        public IEnumerator ShowHitEffect(float time)
        {
            _propertyBlock.SetFloat(ShaderProperties._HitEffect, 1);
            _hitAnimationTimer.Start(time/2);
            while (!_hitAnimationTimer.IsComplete)
            {
                _propertyBlock.SetFloat(ShaderProperties._HitAmount, _hitAnimationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            
            _propertyBlock.SetFloat(ShaderProperties._HitAmount, 1);
            UpdateRenderersPropertyBlock();
            
            _hitAnimationTimer.Start(time/2);
            while (!_hitAnimationTimer.IsComplete)
            {
                _propertyBlock.SetFloat(ShaderProperties._HitAmount,1 - _hitAnimationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            
            _propertyBlock.SetFloat(ShaderProperties._HitAmount, 0);
            _propertyBlock.SetFloat(ShaderProperties._HitEffect, 0);
            UpdateRenderersPropertyBlock();
        }
        
        
        public Bounds GetBounds()
        {
            if (_cachedBounds.HasValue)
            {
                return _cachedBounds.Value;
            }

            Bounds bounds = _renderers[0].bounds;

            foreach (var r in _renderers)
            {
                bounds.Encapsulate(r.bounds);
            }

            _cachedBounds = bounds;
            return bounds;
        }

        private Bounds GetScreenBoundsInWorld()
        {
            Camera cam = Camera.main;

            float height = cam.orthographicSize;
            float width = height * cam.aspect;

            return new Bounds(
                cam.transform.position,
                new Vector3(width * 2, height * 2, 0));
        }

        public IEnumerator ShowVerticalDissolveEffect(float time)
        {
            Bounds bounds = GetBounds();
            
            _propertyBlock.SetFloat(ShaderProperties._BoundsTop, bounds.max.y);
            _propertyBlock.SetFloat(ShaderProperties._BoundsBottom, bounds.min.y);
           
            _verticalDissolveAnimationTimer.Start(time);
            _propertyBlock.SetFloat(ShaderProperties._VerticalDissolve, 1);
            UpdateRenderersPropertyBlock();
            while (!_verticalDissolveAnimationTimer.IsComplete)
            {
                _propertyBlock.SetFloat(ShaderProperties._VerticalDissolveAmount, _verticalDissolveAnimationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            UpdateRenderersPropertyBlock();
        }
        
        
        public IEnumerator ShowHorizontalDissolveEffect(float time, float targetValue, bool inversed)
        {
            Bounds screenBounds = GetScreenBoundsInWorld();
            var startValue = targetValue == 1? 0 : 1;
            
            
            _propertyBlock.SetFloat(ShaderProperties._BoundsLeft, screenBounds.min.x);
            _propertyBlock.SetFloat(ShaderProperties._BoundsRight, screenBounds.max.x);
           
            _horizontalDissolveAnimationTimer.Start(time);
            _propertyBlock.SetFloat(ShaderProperties._HorizontalDissolve, 1);
            _propertyBlock.SetFloat(ShaderProperties._HorizontalDissolveIversed, inversed ? 1 : 0);
            UpdateRenderersPropertyBlock();
            while (!_horizontalDissolveAnimationTimer.IsComplete)
            {
                var value = Mathf.Lerp(startValue, targetValue, _horizontalDissolveAnimationTimer.Progress);
                _propertyBlock.SetFloat(ShaderProperties._HorizontalDissolveAmount, value);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            UpdateRenderersPropertyBlock();
        }
        
        private void UpdateRenderersPropertyBlock()
        {
            foreach (var renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.SetPropertyBlock(_propertyBlock);
                }
            }
        }
        
    }
}
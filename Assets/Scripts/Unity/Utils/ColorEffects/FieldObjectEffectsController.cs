using System.Collections;
using Unity.Utils.Time;
using UnityEngine;

namespace Utils.ColorEffects
{
    public class FieldObjectEffectsController : MonoBehaviour
    {
        
        [SerializeField, HideInInspector]
        private SpriteRenderer[] _renderers;


        private MaterialPropertyBlock _propertyBlock;

        private static class ShaderProperties
        {
            public static readonly int _Dissolve = Shader.PropertyToID(nameof(_Dissolve));
            public static readonly int _DissolveAmount = Shader.PropertyToID(nameof(_DissolveAmount));
            public static readonly int _DissolveVertical = Shader.PropertyToID(nameof(_DissolveVertical));
            public static readonly int _BoundsTop = Shader.PropertyToID(nameof(_BoundsTop));
            public static readonly int _BoundsBottom = Shader.PropertyToID(nameof(_BoundsBottom));
            public static readonly int _HitEffect = Shader.PropertyToID(nameof(_HitEffect));
            public static readonly int _HitAmount = Shader.PropertyToID(nameof(_HitAmount));
        }

        private Timer _animationTimer = new(TimeType.Unscaled);
        
        private void OnValidate()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }
        
        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            /*var boundsTopId = Shader.PropertyToID("_BoundsTop");
            var boundsBottomId = Shader.PropertyToID("_BoundsBottom");
            Bounds bounds = _renderers[0].bounds;


            foreach (var r in _renderers)
                bounds.Encapsulate(r.bounds);
            _propertyBlock.SetFloat(boundsTopId, bounds.max.y);
            _propertyBlock.SetFloat(boundsBottomId, bounds.min.y);
            foreach (var renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.SetPropertyBlock(_propertyBlock);
                }
            }*/
        }
        
        public IEnumerator ShowDissolveEffect(float time)
        {
            _animationTimer.Start(time);
            _propertyBlock.SetFloat(ShaderProperties._Dissolve, 1);
            while (!_animationTimer.IsComplete)
            {
                //нужно передавать параметр bound в шейдер для корректного применения к разным спрайтоам одного юнита
                /*Bounds bounds = _renderers[0].bounds;
                foreach (var r in _renderers)
                    bounds.Encapsulate(r.bounds);
                _propertyBlock.SetFloat(boundsTopId, bounds.max.y);
                _propertyBlock.SetFloat(boundsBottomId, bounds.min.y);*/
                
                _propertyBlock.SetFloat(ShaderProperties._DissolveAmount, _animationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            _propertyBlock.SetFloat(ShaderProperties._Dissolve, 0);
            UpdateRenderersPropertyBlock();
        }
        
        public IEnumerator ShowHitEffect(float time)
        {
            _propertyBlock.SetFloat(ShaderProperties._HitEffect, 1);
            _animationTimer.Start(time/2);
            while (!_animationTimer.IsComplete)
            {
                _propertyBlock.SetFloat(ShaderProperties._HitAmount, _animationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            
            _propertyBlock.SetFloat(ShaderProperties._HitAmount, 1);
            UpdateRenderersPropertyBlock();
            
            _animationTimer.Start(time/2);
            while (!_animationTimer.IsComplete)
            {
                _propertyBlock.SetFloat(ShaderProperties._HitAmount,1 - _animationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            
            _propertyBlock.SetFloat(ShaderProperties._HitAmount, 0);
            _propertyBlock.SetFloat(ShaderProperties._HitEffect, 0);
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
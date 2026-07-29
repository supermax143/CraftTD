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
            public static readonly int _VerticalDissolve = Shader.PropertyToID(nameof(_VerticalDissolve));
            public static readonly int _VerticalDissolveAmount = Shader.PropertyToID(nameof(_VerticalDissolveAmount));
            public static readonly int _BoundsTop = Shader.PropertyToID(nameof(_BoundsTop));
            public static readonly int _BoundsBottom = Shader.PropertyToID(nameof(_BoundsBottom));
            public static readonly int _HitEffect = Shader.PropertyToID(nameof(_HitEffect));
            public static readonly int _HitAmount = Shader.PropertyToID(nameof(_HitAmount));
        }

        private readonly Timer _hitAnimationTimer = new(TimeType.Scaled);
        private readonly Timer _dissolveAnimationTimer = new(TimeType.Scaled);
        private readonly Timer _verticalDissolveAnimationTimer = new(TimeType.Scaled);
        
        private void OnValidate()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
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
                //нужно передавать параметр bound в шейдер для корректного применения к разным спрайтоам одного юнита
                /*Bounds bounds = _renderers[0].bounds;
                foreach (var r in _renderers)
                    bounds.Encapsulate(r.bounds);
                _propertyBlock.SetFloat(boundsTopId, bounds.max.y);
                _propertyBlock.SetFloat(boundsBottomId, bounds.min.y);*/
                
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

        [ContextMenu("Show VDissolve")]
        public void ShowVDissolve()
        {
            StartCoroutine(ShowVerticalDissolveEffect(2));
            StartCoroutine(ShowDissolveEffect(2));
        }
        
        public IEnumerator ShowVerticalDissolveEffect(float time)
        {
            Bounds bounds = _renderers[0].bounds;

            foreach (var r in _renderers)
            {
                bounds.Encapsulate(r.bounds);
            }
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
            //_propertyBlock.SetFloat(ShaderProperties._VerticalDissolve, 0);
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
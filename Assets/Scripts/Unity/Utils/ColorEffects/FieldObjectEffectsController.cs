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
            
        }
        private int _dissolveId = Shader.PropertyToID("_Dissolve");
        private int _dissolveAmountId = Shader.PropertyToID("_DissolveAmount");
        private int _boundsTopId = Shader.PropertyToID("_BoundsTop");
        private int _boundsBottomId = Shader.PropertyToID("_BoundsBottom");
        private int _hitEffectId = Shader.PropertyToID("_HitEffect");
        private int _hitAmountId = Shader.PropertyToID("_HitAmount");
        
        
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
            _propertyBlock.SetFloat(_dissolveId, 1);
            while (!_animationTimer.IsComplete)
            {
                //нужно передавать параметр bound в шейдер для корректного применения к разным спрайтоам одного юнита
                /*Bounds bounds = _renderers[0].bounds;
                foreach (var r in _renderers)
                    bounds.Encapsulate(r.bounds);
                _propertyBlock.SetFloat(boundsTopId, bounds.max.y);
                _propertyBlock.SetFloat(boundsBottomId, bounds.min.y);*/
                
                _propertyBlock.SetFloat(_dissolveAmountId, _animationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            _propertyBlock.SetFloat(_dissolveId, 0);
            UpdateRenderersPropertyBlock();
        }
        
        public IEnumerator ShowHitEffect(float time)
        {
            _propertyBlock.SetFloat(_hitEffectId, 1);
            _animationTimer.Start(time/2);
            while (!_animationTimer.IsComplete)
            {
                _propertyBlock.SetFloat(_hitAmountId, _animationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            
            _propertyBlock.SetFloat(_hitAmountId, 1);
            UpdateRenderersPropertyBlock();
            
            _animationTimer.Start(time/2);
            while (!_animationTimer.IsComplete)
            {
                _propertyBlock.SetFloat(_hitAmountId,1 - _animationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            
            _propertyBlock.SetFloat(_hitAmountId, 0);
            _propertyBlock.SetFloat(_hitEffectId, 0);
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
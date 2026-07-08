using System.Collections;
using Unity.Presentation.Components;
using Unity.Utils.Time;
using UnityEngine;
using Utils.ColorEffects;

namespace Unity.Game
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private TintController _tintController;
        [SerializeField, HideInInspector]
        private UnitAnimatorController _unitAnimatorController;
        [SerializeField, HideInInspector]
        private FieldObjectEffectsController _effectsController;
        
        
        private HealthComponent _healthComponent;
        private Color _color;
        private Transform _rootTransform;
        
        /*private int _dissolveId;
        private MaterialPropertyBlock _propertyBlock;
        private int _dissolveAmountId;
        private int _boundsTopId;
        private int _boundsBottomId;
        private int _hitEffectId;
        private Timer _animationTimer = new(TimeType.Unscaled);
        private int _hitAmountId;*/

        
        private void Awake()
        {
            /*_propertyBlock = new MaterialPropertyBlock();
            
            _dissolveId = Shader.PropertyToID("_Dissolve");
            _dissolveAmountId = Shader.PropertyToID("_DissolveAmount");
            _boundsTopId = Shader.PropertyToID("_BoundsTop");
            _boundsBottomId = Shader.PropertyToID("_BoundsBottom");
            
            _hitEffectId = Shader.PropertyToID("_HitEffect");
            _hitAmountId = Shader.PropertyToID("_HitAmount");*/
              
                    
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

        private void OnValidate()
        {
            _tintController = GetComponentInChildren<TintController>();
            _unitAnimatorController = GetComponentInChildren<UnitAnimatorController>();
            _effectsController = GetComponentInChildren<FieldObjectEffectsController>();
            //_renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        public void Initialize(HealthComponent healthComponent, Transform rootTransform)
        {
            _rootTransform = rootTransform;
            _healthComponent = healthComponent;
            _healthComponent.OnDamage += OnDamage;
        }
        

        private void OnDamage(int damage)
        {
            /*if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
            }
            _blinkCoroutine = StartCoroutine(DamageAnimation());*/
            StartCoroutine(_effectsController.ShowHitEffect(.2f));
        }


        /*private IEnumerator DamageAnimation()
        {
            _propertyBlock.SetFloat(_hitEffectId, 1);
            _animationTimer.Start(.1f);
            _tintController.SetTintColor(Color.white);
            while (!_animationTimer.IsComplete)
            {
                _propertyBlock.SetFloat(_hitAmountId, _animationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            
            _propertyBlock.SetFloat(_hitAmountId, 1);
            UpdateRenderersPropertyBlock();
            
            _animationTimer.Start(.1f);
            while (!_animationTimer.IsComplete)
            {
                _propertyBlock.SetFloat(_hitAmountId,1 - _animationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            
            _propertyBlock.SetFloat(_hitAmountId, 0);
            _propertyBlock.SetFloat(_hitEffectId, 0);
            UpdateRenderersPropertyBlock();
            _tintController.SetTintColor(_color);
        }*/

        public void SetColor(Color color)
        {
            _color = color;
            _tintController.SetTintColor(color);
        }

        private void OnDestroy()
        {
            if (_healthComponent != null)
            {
                _healthComponent.OnDamage -= OnDamage;
            }
        }
        
        public void StartWalking()
        {
            _unitAnimatorController.PlayWalk();
        }
        
        public void StartAttacking()
        {
            _unitAnimatorController.PlayAttack();
        }
        
        public void StartIdle()
        {
            _unitAnimatorController.PlayIdle();
        }
        
        [ContextMenu("Die")]
        public void StartDie()
        {
            _unitAnimatorController.PlayDie();
            StartCoroutine(DeathAnimation());
        }
        
        
        
        private IEnumerator DeathAnimation()
        {
            yield return new WaitForSeconds(1);
            yield return _effectsController.ShowDissolveEffect(.7f);
            /*_animationTimer.Start(.7f);
            _propertyBlock.SetFloat(_dissolveId, 1);
            while (!_animationTimer.IsComplete)
            {
                //нужно передавать параметр bound в шейдер для корректного применения к разным спрайтоам одного юнита
                /*Bounds bounds = _renderers[0].bounds;
                foreach (var r in _renderers)
                    bounds.Encapsulate(r.bounds);
                _propertyBlock.SetFloat(boundsTopId, bounds.max.y);
                _propertyBlock.SetFloat(boundsBottomId, bounds.min.y);#1#
                
                _propertyBlock.SetFloat(_dissolveAmountId, _animationTimer.Progress);
                UpdateRenderersPropertyBlock();
                yield return null;
            }
            
            _propertyBlock.SetFloat(_dissolveId, 0);
            UpdateRenderersPropertyBlock();*/
            
        }

        /*private void UpdateRenderersPropertyBlock()
        {
            foreach (var renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.SetPropertyBlock(_propertyBlock);
                }
            }
        }
        */


        /*
        private void Update()
        {
            Bounds bounds = _renderers[0].bounds;

            foreach (var r in _renderers)
                bounds.Encapsulate(r.bounds);
            foreach (var renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.SetPropertyBlock(_propertyBlock);
                }
            }
        }
        */


        public void UpdateSortingByPosition()
        {
            var pos = _rootTransform.position;
            pos.z = pos.y * 0.001f;
            _rootTransform.position = pos;
        }
        
        public void SetRandomFrame()
        {
            _unitAnimatorController.SetRandomFrame();
        }

    }
}
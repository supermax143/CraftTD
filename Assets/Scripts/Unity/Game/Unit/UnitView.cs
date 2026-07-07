using System;
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
        private BlinkEffect _blinkEffect;
        [SerializeField, HideInInspector]
        private UnitAnimatorController _unitAnimatorController;
        [SerializeField, HideInInspector]
        private SpriteRenderer[] _renderers;
        
        
        private HealthComponent _healthComponent;
        private Coroutine _blinkCoroutine;
        private Color _color;
        private Transform _rootTransform;
        private MaterialPropertyBlock _propertyBlock;

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

        private void OnValidate()
        {
            _healthComponent = GetComponentInChildren<HealthComponent>();
            _tintController = GetComponentInChildren<TintController>();
            _blinkEffect = GetComponentInChildren<BlinkEffect>();
            _unitAnimatorController = GetComponentInChildren<UnitAnimatorController>();
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        public void Initialize(HealthComponent healthComponent, Transform rootTransform)
        {
            _rootTransform = rootTransform;
            _healthComponent = healthComponent;
            _healthComponent.OnDamage += OnDamage;
        }
        

        private void OnDamage(int damage)
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
            }
            _blinkCoroutine = StartCoroutine(DamageAnimation());
        }


        private IEnumerator DamageAnimation()
        {
            /*_tintController.SetTintColor(Color.red);
            yield return new WaitForSeconds(0.2f);
            _tintController.SetTintColor(_color);*/
            _tintController.SetTintColor(Color.red);
            foreach (var renderer in _renderers)
            {
                renderer.color = Color.red;
            }
            yield return new WaitForSeconds(0.1f);
            foreach (var renderer in _renderers)
            {
                renderer.color = Color.white;
            }
            _tintController.SetTintColor(_color);
        }

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
            
            var dissolveId = Shader.PropertyToID("_Dissolve");
            var dissolveAmountId = Shader.PropertyToID("_DissolveAmount");
            var boundsTopId = Shader.PropertyToID("_BoundsTop");
            var boundsBottomId = Shader.PropertyToID("_BoundsBottom");
            var timer = new Timer();
            yield return new WaitForSeconds(1);
            timer.Start(.7f);

            _propertyBlock.SetFloat(dissolveId, 1);
            while (!timer.IsComplete)
            {
                /*Bounds bounds = _renderers[0].bounds;
                foreach (var r in _renderers)
                    bounds.Encapsulate(r.bounds);
                _propertyBlock.SetFloat(boundsTopId, bounds.max.y);
                _propertyBlock.SetFloat(boundsBottomId, bounds.min.y);*/
                
                _propertyBlock.SetFloat(dissolveAmountId, timer.Progress);
                foreach (var renderer in _renderers)
                {
                    if (renderer != null)
                    {
                        renderer.SetPropertyBlock(_propertyBlock);
                    }
                }
                yield return null;
            }
            
            _propertyBlock.SetFloat(dissolveId, 0);
            foreach (var renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.SetPropertyBlock(_propertyBlock);
                }
            }
            
        }

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
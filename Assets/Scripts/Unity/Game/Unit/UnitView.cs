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
            MaterialPropertyBlock _propertyBlock = new MaterialPropertyBlock();
            var dissolveId = Shader.PropertyToID("_DissolveAmount");
            var timer = new Timer();
            timer.Start(1);

            while (!timer.IsComplete)
            {
                _propertyBlock.SetFloat(dissolveId, timer.Progress);
                foreach (var renderer in _renderers)
                {
                    if (renderer != null)
                    {
                        renderer.SetPropertyBlock(_propertyBlock);
                    }
                }
                yield return null;
            }
            
            _propertyBlock.SetFloat(dissolveId, 1);
            foreach (var renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.SetPropertyBlock(_propertyBlock);
                }
            }
            
        }

        /*private void Update()
        {
            Bounds bounds = _renderers[0].bounds;

            foreach (var r in _renderers)
                bounds.Encapsulate(r.bounds);
            _propertyBlock.SetFloat("_BoundsTop", bounds.max.y);
            _propertyBlock.SetFloat("_BoundsBottom", bounds.min.y);
            foreach (var renderer in _renderers)
            {
                if (renderer != null)
                {
                    renderer.SetPropertyBlock(_propertyBlock);
                }
            }
        }*/


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
using System;
using System.Collections;
using Unity.Presentation.Components;
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
        private HealthComponent _healthComponent;
        [SerializeField, HideInInspector]
        private UnitAnimator _unitAnimator;
        [SerializeField, HideInInspector]
        private SpriteRenderer[] _renderers;
        
        
        private Coroutine _blinkCoroutine;
        private Color _color;

        
        private void OnValidate()
        {
            _healthComponent = GetComponentInChildren<HealthComponent>();
            _tintController = GetComponentInChildren<TintController>();
            _blinkEffect = GetComponentInChildren<BlinkEffect>();
            _unitAnimator = GetComponentInChildren<UnitAnimator>();
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Start()
        {
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
            _tintController.SetTintColor(Color.red);
            yield return new WaitForSeconds(0.2f);
            _tintController.SetTintColor(_color);
        }

        public void SetColor(Color color)
        {
            _color = color;
            _tintController.SetTintColor(color);
        }

        private void OnDestroy()
        {
            _healthComponent.OnDamage -= OnDamage;
        }
        
        public void StartWalking()
        {
            _unitAnimator.PlayWalk();
        }
        
        public void StartAttacking()
        {
            _unitAnimator.PlayAttack();
        }
        
        public void StartIdle()
        {
            _unitAnimator.PlayIdle();
        }
        
        public void UpdateSortingByPosition()
        {
            var pos = transform.position;
            pos.z = pos.y * 0.001f;
            transform.position = pos;
        }
        
        public void SetRandomFrame()
        {
            _unitAnimator.SetRandomFrame();
        }
    }
}
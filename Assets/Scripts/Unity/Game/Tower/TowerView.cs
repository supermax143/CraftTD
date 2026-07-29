using System;
using System.Collections;
using Unity.Utils.Time;
using UnityEngine;
using Utils.ColorEffects;

namespace Unity.Game
{
    public class TowerView : MonoBehaviour
    {
        
        [SerializeField, HideInInspector]
        private HealthComponent _healthComponent;
        [SerializeField, HideInInspector]
        private FieldObjectEffectsController _effectsController;
        
        private Coroutine _blinkCoroutine;
        private Color _color;

        private void OnValidate()
        {
            _healthComponent = GetComponentInChildren<HealthComponent>();
            _effectsController = GetComponentInChildren<FieldObjectEffectsController>();
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
            yield return _effectsController.ShowHitEffect(.2f);
        }

        public void SetColor(Color color)
        {
        }

        public void ShowExplosion(Action onComplete)
        {
            StartCoroutine(ShowExplosionAnimation(onComplete));
        }

        private IEnumerator ShowExplosionAnimation(Action onComplete)
        {
            var timer = new Timer();
            
            timer.Start(3);
            StartCoroutine(_effectsController.ShowDissolveEffect(3));
            StartCoroutine(_effectsController.ShowVerticalDissolveEffect(3));
            while (!timer.IsComplete)
            {
                
                yield return null;
            }
            onComplete?.Invoke();
        }
    }
}
using System;
using System.Collections;
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
        
        private Coroutine _blinkCoroutine;
        private Color _color;

        private void OnValidate()
        {
            _healthComponent = GetComponentInChildren<HealthComponent>();
            _tintController = GetComponentInChildren<TintController>();
            _blinkEffect = GetComponentInChildren<BlinkEffect>();
        }

        private void Start()
        {
            _healthComponent.OnDamage += OnDamage;
        }

        private void OnDamage()
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
            }
            _blinkCoroutine = StartCoroutine(DamageAnimation());
        }


        private IEnumerator DamageAnimation()
        {
            Debug.Log("start");
            _tintController.SetTintColor(Color.red);
            //yield return _blinkEffect.Show();
            yield return new WaitForSeconds(0.2f);
            _tintController.SetTintColor(_color);
            Debug.Log("finish");
            
        }

        public void SetColor(Color color)
        {
            _color = color;
            _tintController.SetTintColor(color);
        }
        
        
        
    }
}
using System.Collections;
using UnityEngine;
using Utils.ColorEffects;

namespace Unity.Game
{
    public class TowerView : MonoBehaviour
    {
        
        [SerializeField, HideInInspector]
        private HealthComponent _healthComponent;

        private Coroutine _blinkCoroutine;
        private Color _color;

        private void OnValidate()
        {
            _healthComponent = GetComponentInChildren<HealthComponent>();
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
            yield return new WaitForSeconds(0.2f);
            
        }

        public void SetColor(Color color)
        {
        }
    }
}
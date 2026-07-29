using System;
using System.Collections;
using Unity.Infrastructure.Effects;
using Unity.Utils.Time;
using UnityEngine;
using Utils.ColorEffects;
using Zenject;
using Random = UnityEngine.Random;

namespace Unity.Game
{
    public class TowerView : MonoBehaviour
    {
        
        [SerializeField, HideInInspector]
        private HealthComponent _healthComponent;
        [SerializeField, HideInInspector]
        private FieldObjectEffectsController _effectsController;
        
        [Inject] private VisualEffectSpawnManager _effectSpawnManager;
        
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

        [ContextMenu("Test Show Explosion")]
        public void TestShowExplosion()
        {
            ShowExplosion(null);
        }
        
        public void ShowExplosion(Action onComplete)
        {
            StartCoroutine(ShowExplosionAnimation(onComplete));
        }

        private IEnumerator ShowExplosionAnimation(Action onComplete)
        {
            var timer = new Timer();
            var explosionTimer = new Timer();
            var bounds = _effectsController.GetBounds();
            timer.Start(3);
            explosionTimer.Start(0.2f);
            StartCoroutine(_effectsController.ShowDissolveEffect(3));
            StartCoroutine(_effectsController.ShowVerticalDissolveEffect(3));
            while (!timer.IsComplete)
            {
                if (explosionTimer.IsComplete)
                {
                    var randomX = Random.Range(bounds.min.x, bounds.max.x);
                    var randomY = Random.Range(bounds.min.y, bounds.max.y);
                    var randomPosition = new Vector3(randomX, randomY, transform.position.z);
                    _effectSpawnManager.SpawnRandomExplosion(randomPosition, transform);
                    explosionTimer.Start(0.2f);
                }
                yield return null;
            }
            onComplete?.Invoke();
        }
    }
}
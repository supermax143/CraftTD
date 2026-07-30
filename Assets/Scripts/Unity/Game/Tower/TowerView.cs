using System;
using System.Collections;
using Unity.Infrastructure.Camera;
using Unity.Infrastructure.Effects;
using Unity.Settings;
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
        [SerializeField]
        private SpriteRenderer _boundsSprite;
        
        
        [Inject] private VisualEffectSpawnManager _effectSpawnManager;
        [Inject] private ICameraController _cameraController;
        [Inject] private GameSettings _gameSettings;
        
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

        private Bounds GetBounds() => _boundsSprite.bounds;


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
            var bounds = GetBounds();
            var time = _gameSettings.ExplosionAnimationTime;
            timer.Start(time);
            explosionTimer.Start(0.2f);
            StartCoroutine(_effectsController.ShowDissolveEffect(time));
            StartCoroutine(_effectsController.ShowVerticalDissolveEffect(time));
            while (!timer.IsComplete)
            {
                if (explosionTimer.IsComplete)
                {
                    var randomX = Random.Range(bounds.min.x, bounds.max.x);
                    var maxY = bounds.max.y - (bounds.size.y * timer.Progress);
                    var randomY = Random.Range(bounds.min.y, maxY);
                    var randomPosition = new Vector3(randomX, randomY, transform.position.z);
                    _effectSpawnManager.SpawnRandomExplosion(randomPosition, transform);
                    _cameraController.ShakeCamera();
                    explosionTimer.Start(0.2f);
                }
                yield return null;
            }
            onComplete?.Invoke();
        }
    }
}
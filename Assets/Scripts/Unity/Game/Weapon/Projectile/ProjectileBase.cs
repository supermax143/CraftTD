using System.Collections;
using DG.Tweening;
using Unity.Infrastructure.Effects;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game.Projectile
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        [Header("Flight")]
        [SerializeField] private AnimationCurve _flyArc;
        [SerializeField] private float _speed = 15f;
        [SerializeField] private float _rotationSpeed = 0f;
        [SerializeField] private bool _isTopTarget = true;
        [SerializeField] private PopupType _hitEffect = PopupType.None;
        
        private Coroutine _moveCoroutine;
        protected AttackTargetBase _target;
        private readonly Timer _timer = new();
        protected float _damage;
        protected Vector3 _direction;
        protected PopupSpawnManager _effectsSpawner;


        public void Launch(AttackTargetBase target, float damage, PopupSpawnManager popupSpawnManager)
        {
            _effectsSpawner = popupSpawnManager;
            _direction = (target.transform.position - transform.position).normalized;
            DOTween.Kill(transform);

            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }

            transform.localScale = Vector3.one;

            _target = target;
            _damage = damage;

            _moveCoroutine = StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            var startPosition = transform.position;

            if (!_target.TryGetAttackPosition(_isTopTarget, out var targetPosition))
            {
                Destroy(gameObject);
                yield break;
            }

            var time = Vector3.Distance(startPosition, targetPosition) / _speed;

            _timer.Start(time);

            while (!_timer.IsComplete)
            {
                if (!_target.TryGetAttackPosition(false, out targetPosition))
                {
                    Destroy(gameObject);
                    yield break;
                }

                var pos = Vector3.Lerp(startPosition, targetPosition, _timer.Progress);
                pos.y += _flyArc.Evaluate(_timer.Progress);

                transform.position = pos;
                
                var rotation = transform.localRotation.eulerAngles;
                rotation.z += _rotationSpeed * Time.deltaTime * -Mathf.Sign(_direction.x);
                transform.localRotation = Quaternion.Euler(rotation) ;
                yield return null;
            }

            transform.position = targetPosition;

            ApplyDamage(startPosition);
            ShowHitEffect();
            OnFlightComplete();
        }

        protected virtual void OnFlightComplete()
        {
            Destroy(gameObject);
        }
        
        protected virtual void ShowHitEffect()
        {
            if (_hitEffect == PopupType.None)
            {
                return;
            }
            _effectsSpawner.SpawnEffect(_hitEffect, transform.position, transform.parent);
        }
        
        protected virtual void ApplyDamage(Vector3 startPosition)
        {
            
            if (_target == null || _target.HealthComponent == null)
            {
                return;
            }
            _target.SetLastAttackDirection(_target.transform.position - startPosition);
            _target.HealthComponent.TakeDamage(_damage);
        }
    }
}
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

        private float _damage;
        private Coroutine _moveCoroutine;
        private AttackTargetBase _target;
        private readonly Timer _timer = new();
        protected Vector3 _direction;
        protected EffectSpawnManager _effectSpawnManager;

        protected abstract void OnFlyghtComplete();

        public void Launch(AttackTargetBase target, float damage, EffectSpawnManager effectSpawnManager)
        {
            _effectSpawnManager = effectSpawnManager;
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

            if (!_target.TryGetAttackPosition(transform.position, false, out var targetPosition))
            {
                Destroy(gameObject);
                yield break;
            }

            var time = Vector3.Distance(startPosition, targetPosition) / _speed;

            _timer.Start(time);

            while (!_timer.IsComplete)
            {
                if (!_target.TryGetAttackPosition(transform.position, false, out targetPosition))
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
                Debug.Log(transform.rotation);
                yield return null;
            }

            transform.position = targetPosition;

            if (_target != null && _target.HealthComponent != null)
            {
                _target.SetLastAttackDirection(_target.transform.position - startPosition);
                _target.HealthComponent.TakeDamage(_damage);
            }

            OnFlyghtComplete();
        }

        
    }
}
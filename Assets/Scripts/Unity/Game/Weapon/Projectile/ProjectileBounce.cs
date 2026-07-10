using System.Collections;
using DG.Tweening;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game.Projectile
{
    public class ProjectileBounce : ProjectileBase
    {
       
        
        [Header("Bounce")]
        [SerializeField] private int _bounceCount = 4;
        [SerializeField] private float _bounceDistance = 1.2f;
        [SerializeField] private float _bounceHeight = 0.4f;
        [SerializeField] private float _bounceDuration = 0.18f;

        /*private float _damage;
        private Coroutine _moveCoroutine;
        private AttackTargetBase _target;
        private readonly Timer _timer = new();
        private Vector3 _direction;*/
        
        /*public void Launch(AttackTargetBase target, float damage)
        {
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

            PlayBounceAndDestroy(startPosition);
        }*/

        protected override void OnFlyghtComplete()
        {
            Vector3 direction = _direction * -1;

            Sequence sequence = DOTween.Sequence();

            Vector3 currentPos = transform.position;

            float distance = _bounceDistance;
            float height = _bounceHeight;
            float duration = _bounceDuration;

            int bounceCount = Random.Range(
                Mathf.Max(1, _bounceCount - 2),
                _bounceCount + 1);

            for (int i = 0; i < bounceCount; i++)
            {
                // Разброс по горизонтали и немного вверх/вниз
                direction = Quaternion.Euler(
                    Random.Range(-10f, 10f),   // Pitch
                    Random.Range(-20f, 20f),   // Yaw
                    0f) * direction;

                Vector3 nextPos = currentPos + direction * distance;

                float spin = Random.Range(220f, 520f);
                if (Random.value > 0.5f)
                    spin = -spin;

                sequence.Append(
                    transform.DOJump(nextPos, height, 1, duration)
                        .SetEase(Ease.OutQuad));

               

                currentPos = nextPos;

                // Затухание
                distance *= Random.Range(0.45f, 0.65f);
                height *= Random.Range(0.45f, 0.65f);
                duration *= Random.Range(0.90f, 1.05f);
            }

            /*sequence.Append(
                transform.DOScale(Vector3.zero, 0.12f)
                    .SetEase(Ease.InBack));*/

            sequence.OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }
    }
}
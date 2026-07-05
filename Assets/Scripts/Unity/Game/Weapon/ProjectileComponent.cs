using System.Collections;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game.Projectile
{
    public class ProjectileComponent : MonoBehaviour
    {

        [SerializeField]
        private float _speed = 15;
        
        private float _damage;
       
        private Coroutine _moveCoroutine;
        private AttackTargetBase _target;
        private Timer _timer = new();
        
        public void Launch(AttackTargetBase target, float damage)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _target = target;
            _damage = damage;
            _moveCoroutine = StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            var startPosition = transform.position;

            if (!_target.TryGetAttackPosition(transform.position, out var targetPosition))
            {
                Destroy(gameObject);
                yield break;
            }
            var time = Vector3.Distance(transform.position, targetPosition) / _speed;
            _timer.Start(time);
            while (!_timer.IsComplete)
            {
                if (!_target.TryGetAttackPosition(transform.position, out targetPosition))
                {
                    Destroy(gameObject);
                    yield break;
                }
                transform.position = Vector3.Lerp(startPosition, targetPosition, _timer.Progress);
                yield return null;
            }
            transform.position = targetPosition;
            if (_target != null || _target.HealthComponent != null)
            {
                _target.SetLastAttackDirection(_target.transform.position - startPosition);
                _target.HealthComponent.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
    }
    
    
}
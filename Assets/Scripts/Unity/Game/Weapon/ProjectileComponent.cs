using System.Collections;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game.Projectile
{
    public class ProjectileComponent : MonoBehaviour
    {

        private float _speed;
        private float _damage;
       
        private Coroutine _moveCoroutine;
        private AttackTargetBase _target;
        private Timer _timer = new();
        
        public void Launch(AttackTargetBase target, float speed, float damage)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _target = target;
            _speed = speed;
            _damage = damage;
            _moveCoroutine = StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            var startPosition = transform.position;
            var targetPosition = _target.GetClosestPosition(transform.position);
            var time = Vector3.Distance(transform.position, targetPosition) / _speed;
            _timer.Start(time);
            while (!_timer.IsComplete)
            {
                if (_target == null)
                {
                    Destroy(gameObject);
                }
                transform.position = Vector3.Lerp(startPosition, targetPosition, _timer.Progress);
                yield return null;
            }
            transform.position = targetPosition;
            _target.HealthComponent.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
    
    
}
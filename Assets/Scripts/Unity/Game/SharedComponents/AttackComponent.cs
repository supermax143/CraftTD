using System.Collections;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game
{
    public class AttackComponent : MonoBehaviour
    {
        private AttackData _data;

        public AttackData Data => _data;

        private Coroutine _attackCoroutine;

        private AttackTarget _target;
        
        public void Initialize(AttackData data)
        {
            _data = data;
        }
        
        public void Activate(AttackTarget target)
        {
            _target = target;
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
            _attackCoroutine = StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            if (_target == null)
            {
                Debug.LogError($"{this.GetType().Name} Target is null");
                yield break;
            }
            transform.LookAt(_target.transform);//TODO: перенести в MoveComponent
            _target.Health.TakeDamage(_data.Damage);
            yield return new WaitForSeconds(_data.Cooldown);
            _attackCoroutine = StartCoroutine(Attack());
        }
        
        public void Deactivate()
        {
            StopCoroutine(_attackCoroutine);
        }
        
        public bool CheckRange(AttackTarget target)
        {
            var position = transform.position;
            var targetPosition = target.GetClosestPosition(position);
            var distance = Vector3.Distance(position, targetPosition);
            return distance <= _data.Range;
        }
        
    }
}
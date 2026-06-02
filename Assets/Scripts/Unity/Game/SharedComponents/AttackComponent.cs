using System.Collections;
using Unity.Game.Projectile;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game
{
    public class AttackComponent : MonoBehaviour
    {
        
        [SerializeField]
        private Weapon _weapon;
        
        private AttackData _data;

        public AttackData Data => _data;

        private Coroutine _attackCoroutine;

        private AttackTarget _target;
        private UnitController _unit;

        private void OnValidate()
        {
            _weapon = GetComponentInChildren<Weapon>();
        }
        
        public void Initialize(AttackData data, UnitController unit)
        {
            _data = data;
            _unit = unit;
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

            _unit.Move.RotateTo(_target.GetClosestPosition(_unit.transform.position));
            _weapon.Attack(_target, _data.Damage);
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
using System.Collections;
using Unity.Game.Attributes.Specific;
using Unity.Game.Projectile;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game
{
    public class AttackComponent : GameEntity
    {
        
        [SerializeField, HideInInspector]
        private MoveComponent _moveComponent;
        
        [SerializeField]
        private Weapon _weapon;
        
        
        [SerializeField] 
        private DamageAttribute _damage;
        [SerializeField] 
        private AttackRangeAttribute _attackRange;
        [SerializeField] 
        private AttackSpeedAttribute _attackSpeed;
        [SerializeField] 
        private AttackCooldownAttribute _attackCooldown;
        

        public float AttackRange => _attackRange.ValueModified;
        public float AttackSpeed => _attackSpeed.ValueModified;
        public float AttackCooldown => _attackCooldown.ValueModified;
        public float Damage => _damage.ValueModified;
        
        private Coroutine _attackCoroutine;

        private AttackTarget _target;

        private void OnValidate()
        {
            _weapon = GetComponentInChildren<Weapon>();
            _moveComponent = GetComponentInChildren<MoveComponent>();
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

            _moveComponent.RotateTo(_target.GetClosestPosition(transform.position));
            _weapon.Attack(_target, Damage);
            yield return new WaitForSeconds(AttackCooldown);
            _attackCoroutine = StartCoroutine(Attack());
        }
        
        public void Deactivate()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
        }
        
        public bool CheckRange(AttackTarget target)
        {
            var position = transform.position;
            var targetPosition = target.GetClosestPosition(position);
            var distance = Vector3.Distance(position, targetPosition);
            return distance <= AttackRange;
        }

        
    }
}
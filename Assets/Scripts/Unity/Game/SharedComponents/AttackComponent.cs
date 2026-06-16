using System.Collections;
using Unity.Game.Attributes.Specific;
using Unity.Game.Projectile;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game
{
    public class AttackComponent : GameComponent
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
       
        

        public float AttackRange => _attackRange.ValueModified;
        public float AttackSpeed => _attackSpeed.ValueModified;
        public float Damage => _damage.ValueModified;
        
        private Coroutine _attackCoroutine;

        private AttackTargetBase _target;

        private void OnValidate()
        {
            _weapon = GetComponentInChildren<Weapon>();
            _moveComponent = GetComponentInChildren<MoveComponent>();
        }
        
       
        public void Activate(AttackTargetBase target)
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
            yield return new WaitForSeconds(AttackSpeed);
            _moveComponent.RotateTo(_target.GetClosestPosition(transform.position));
            if (_target == null)
            {
                Debug.LogError($"{this.GetType().Name} Target is null");
                yield break;
            }

            _weapon.Attack(_target, Damage);
            _attackCoroutine = StartCoroutine(Attack());
        }
        
        public void Deactivate()
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
        }
        
        public bool CheckRange(AttackTargetBase target)
        {
            var position = transform.position;
            var targetPosition = target.GetClosestPosition(position);
            var distance = Vector3.Distance(position, targetPosition);
            return distance <= AttackRange;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 position = transform.position + Vector3.up * 3f;
            Color healthColor = Color.white;
            UnityEditor.Handles.Label(position, $"d: {Damage}", new GUIStyle
            {
                normal = { textColor = healthColor },
                fontSize = 12,
                fontStyle = FontStyle.Bold
            });
        }
#endif
    }
}
using System;
using System.Collections;
using Unity.Game.Attributes.Specific;
using Unity.Game.Projectile;
using Unity.Infrastructure.GameEvents;
using Unity.Presentation.Components;
using Unity.Utils.Time;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class AttackComponent : GameComponent
    {
        
        public event Action OnAttack;
        
        [SerializeField]
        private Weapon _weapon;
        
        
        [SerializeField] 
        private DamageAttribute _damage;
        [SerializeField] 
        private AttackRangeAttribute _attackRange;
        [SerializeField] 
        private AttackSpeedAttribute _attackSpeed;
        [SerializeField, HideInInspector]
        private UnitAnimationEvents _animationEvents;
       
        [Inject] private IGameEventsBus _gameEventsBus;

        public float AttackRange => _attackRange.BaseValueModified;
        public float AttackSpeed => _attackSpeed.BaseValueModified;
        public float Damage => _damage.BaseValueModified;
        
        private Coroutine _attackCoroutine;
        
        private AttackTargetBase _target;
        private UnitView _view;
        private MoveComponentBase _moveComponent;


        private void OnValidate()
        {
            _weapon = GetComponentInChildren<Weapon>();
            _animationEvents = GetComponentInChildren<UnitAnimationEvents>();
        }

        public void Initialize(UnitView view, MoveComponentBase moveComponent)
        {
            _view = view;
            _moveComponent = moveComponent;
        }
        
        private void Start()
        {
            if (_animationEvents != null)
            {
                _animationEvents.OnAttackActivate += AttackActivate;
                _animationEvents.OnAttackAnimationStart += AttackAnimationStart;
            }
        }

        private void AttackAnimationStart()
        {
            if(_target == null)
            {
                return;
            }
            _moveComponent.RotateTo(_target.transform.position);
        }

        private void AttackActivate()
        {
            if (_target == null ||
                _target.IsDead  ||
                !_target.TryGetAttackPosition(false, out var targetPosition))
            {
                return;
            }
            
           
            var direction = (targetPosition - transform.position).normalized;
            _weapon.Attack(_target, Damage, direction);
            _gameEventsBus.TriggerEvent(new DamageAppliedEvent(Damage, _target.Faction));
            OnAttack?.Invoke();
        }

        public void ChangeTarget(AttackTargetBase target)
        {
            _target = target;
        }
        
        public void Activate(AttackTargetBase target)
        {

            _target = target;
            if (_animationEvents != null)
            {
                _view.StartAttacking();
                return;
            }
            //кейс без аниматора
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
            _attackCoroutine = StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(AttackSpeed);
            if (!_target.TryGetClosestPosition(transform.position, out var targetPosition))
            {
                yield return null;
            }
            _moveComponent.RotateTo(targetPosition);
            if (_target == null || _target.IsDead)
            {
                Debug.LogError($"{this.GetType().Name} Target is null");
                yield break;
            }

            _weapon.Attack(_target, Damage, Vector3.zero);
            OnAttack?.Invoke();
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
            Vector2 position = transform.position;
            if (!target.TryGetClosestPosition(position, out var targetPosition))
            {
                return false;
            }
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
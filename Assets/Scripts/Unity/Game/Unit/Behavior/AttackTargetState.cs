using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние атаки выбранной цели
    /// </summary>
    public class AttackTargetState : UnitState
    {
        private float _attackTimer;

        public override void Enter()
        {
            _attackTimer = 0f;
        }

        public override void Update()
        {
            if (_stateManager.CurrentTarget == null)
            {
                ChangeState<MoveToTowerState>();
                return;
            }

            var distance = Vector3.Distance(_unit.transform.position, _stateManager.CurrentTarget.transform.position);
            if (distance > _unit.Attack.Range)
            {
                ChangeState<MoveToTargetState>();
                return;
            }

            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _unit.Attack.Cooldown)
            {
                _attackTimer = 0f;
                Attack();
            }
        }

        private void Attack()
        {
            if (_stateManager.CurrentTarget == null) return;
            
            var targetHealth = _stateManager.CurrentTarget.GetComponent<UnitController>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(_unit.Attack.Damage);
            }
        }
    }
}

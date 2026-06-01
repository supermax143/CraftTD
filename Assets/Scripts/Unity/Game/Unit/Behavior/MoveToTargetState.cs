using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние движения к выбранной цели
    /// </summary>
    public class MoveToTargetState : UnitState
    {
        [SerializeField] private float _moveSpeed = 3f;

        public override void Update()
        {
            if (_stateManager.CurrentTarget == null)
            {
                ChangeState<MoveToTowerState>();
                return;
            }

            var distance = Vector3.Distance(_unit.transform.position, _stateManager.CurrentTarget.transform.position);
            var attackRange = _unit.Attack.Range;

            if (distance <= attackRange)
            {
                ChangeState<AttackTargetState>();
                return;
            }

            MoveToTarget(_stateManager.CurrentTarget.transform.position);
        }

        private void MoveToTarget(Vector3 targetPosition)
        {
            var direction = (targetPosition - _unit.transform.position).normalized;
            _unit.transform.position += direction * _moveSpeed * Time.deltaTime;
            _unit.transform.LookAt(targetPosition);
        }
    }
}

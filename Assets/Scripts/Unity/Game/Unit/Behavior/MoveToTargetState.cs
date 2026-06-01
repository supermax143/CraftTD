using System.Linq;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние движения к выбранной цели
    /// </summary>
    public class MoveToTargetState : UnitState
    {
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _detectionRadius = 2f;
        [SerializeField] private float _detectionInterval = .5f;
        
        private readonly Timer _detectionTimer = new();

        public override void Enter()
        {
            _detectionTimer.Start(_detectionInterval);
        }

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

            if (_detectionTimer.IsComplete)
            {
                CheckForNearTargets();
                _detectionTimer.Start(_detectionInterval);
            }
            
            MoveToTarget(_stateManager.CurrentTarget.transform.position);
        }

        private void MoveToTarget(Vector3 targetPosition)
        {
            var direction = (targetPosition - _unit.transform.position).normalized;
            var delta = direction * _moveSpeed * Time.deltaTime;
            delta.y = 0;
            _unit.transform.position += delta;
            _unit.transform.LookAt(targetPosition);
        }
        
        private void CheckForNearTargets()
        {
            var colliders = Physics.OverlapSphere(_unit.transform.position, _detectionRadius)
                .OrderByDescending(x => Vector3.Distance(x.transform.position, _unit.transform.position))
                .ToArray();
            
            foreach (var collider in colliders)
            {
                var attackTarget = collider.GetComponent<AttackTarget>();
                if (attackTarget != null && attackTarget.Faction == _unit.OpponentFaction)
                {
                    _stateManager.CurrentTarget = attackTarget;
                    return;
                }
            }
        }
        
    }
}

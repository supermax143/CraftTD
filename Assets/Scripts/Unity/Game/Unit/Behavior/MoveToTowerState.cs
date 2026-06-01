using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние движения к вражеской башне с поиском целей
    /// </summary>
    public class MoveToTowerState : UnitState
    {
        [SerializeField] private float _moveSpeed = 3f;
        [SerializeField] private float _detectionRadius = 2f;
        [SerializeField] private float _detectionInterval = 0.5f;
        
        private float _detectionTimer;
        private AttackTarget _targetTower;

        public override void Initialize(UnitStateManager stateManager, UnitController unit)
        {
            base.Initialize(stateManager, unit);
        }

        public override void Enter()
        {
            _detectionTimer = 0f;
            FindTargetTower();
        }

        public override void Update()
        {
            if (_targetTower == null)
            {
                FindTargetTower();
                if (_targetTower == null) return;
            }

            MoveToTarget(_targetTower.transform.position);
            
            _detectionTimer += Time.deltaTime;
            if (_detectionTimer >= _detectionInterval)
            {
                _detectionTimer = 0f;
                CheckForTargets();
            }
        }

        private void FindTargetTower()
        {
            if (_stateManager.GameController.TryGetOpponentTower(_unit.OpponentFaction, out var tower))
            {
                _targetTower = tower;
            }
        }

        private void MoveToTarget(Vector3 targetPosition)
        {
            var direction = (targetPosition - _unit.transform.position).normalized;
            _unit.transform.position += direction * _moveSpeed * Time.deltaTime;
            _unit.transform.LookAt(targetPosition);
        }

        private void CheckForTargets()
        {
            var colliders = Physics.OverlapSphere(_unit.transform.position, _detectionRadius);//TODO: Use non-allocating method 'OverlapSphereNonAlloc'
            foreach (var collider in colliders)
            {
                var attackTarget = collider.GetComponent<AttackTarget>();
                if (attackTarget != null && attackTarget.Faction == _unit.OpponentFaction)
                {
                    _stateManager.CurrentTarget = attackTarget;
                    ChangeState<MoveToTargetState>();
                    return;
                }
            }
        }
    }
}

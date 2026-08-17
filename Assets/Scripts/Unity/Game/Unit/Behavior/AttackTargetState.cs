using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние атаки выбранной цели
    /// </summary>
    public class AttackTargetState : UnitState
    {

        public override void Enter()
        {
            _unit.Attack.OnAttack += OnAttackComplete;
            _unit.Attack.Activate(_stateManager.CurrentTarget);
        }

        private void OnAttackComplete()
        {
            var curTarget = _stateManager.CurrentTarget;
            if (curTarget == null || curTarget.Type == TargetType.Unit)
            {
                return;
            }

            if (_unit.Faction == Faction.Player)
            {
                Debug.Log("asdsd");
            }
            
            var targetSearch = _unit.TargetSearchComponent;
            if (targetSearch.TryGetClosestTarget(out var target) && target.Type == TargetType.Tower)
            {
               return;
            }
            _stateManager.CurrentTarget = target;
            _unit.Attack.ChangeTarget(_stateManager.CurrentTarget);
        }


        public override void UpdateState()
        {
            var curTarget = _stateManager.CurrentTarget;
            if (curTarget == null || curTarget.IsDead)
            {
                ChangeState<SearchTargetState>();
                return;
            }
            
            if (!_unit.Attack.CheckRange(curTarget))
            {
                ChangeState<MoveToTargetState>();
            }
            
        }

        
        public override void Exit()
        {
            base.Exit();
            _unit.Attack.OnAttack -= OnAttackComplete;
            _unit.Attack.Deactivate();
        }

    }
}

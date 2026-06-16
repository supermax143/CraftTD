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
            _unit.Attack.Activate(_stateManager.CurrentTarget);
            _unit.View.StartAttacking();
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
            _unit.Attack.Deactivate();
        }

    }
}

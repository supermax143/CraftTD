using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние оборонительной стойки - юнит стоит на месте и атакует врагов в радиусе
    /// </summary>
    public class DefenseStanceState : UnitState
    {
        private bool _isActive;

        public override void Enter()
        {
            _isActive = true;
            _stateManager.GameController.OnDefenseStanceSwitched += OnDefenceStanceSwitched;
            _unit.View.StartIdle();
        }

        private void OnDefenceStanceSwitched(UnitTier tier, bool active)
        {
            if (_unit.Tier != tier)
            {
                return;
            }
            ChangeState<SearchTargetState>();
        }

        public override void UpdateState()
        {
            if (!_isActive)
            {
                return;
            }

            var targetSearch = _unit.TargetSearchComponent;

            if (targetSearch.TryGetClosestTarget(out var target) && _unit.Attack.CheckRange(target))
            {
                _stateManager.CurrentTarget = target;
                ChangeState<AttackTargetState>();
            }
            
        }

        public override void Exit()
        {
            _isActive = false;
            _stateManager.GameController.OnDefenseStanceSwitched -= OnDefenceStanceSwitched;
        }

        
    }
}

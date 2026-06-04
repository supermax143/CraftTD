using System.Linq;
using UnityEngine;

namespace Unity.Game
{
    /// <summary>
    /// Состояние Поиска цели
    /// </summary>
    public class SearchTargetState : UnitState
    {
        
        public override void Enter()
        {
            var targetSearch = _unit.TargetSearchComponent;
            if (targetSearch.TryGetClosestTarget(out var target) || targetSearch.TryGetTargetTower(out target))
            {
                _stateManager.CurrentTarget = target;
                ChangeState<MoveToTargetState>();
                return;
            }
            
            Debug.LogError($"{this.GetType().Name} Target not found");
        }
        
    }
}

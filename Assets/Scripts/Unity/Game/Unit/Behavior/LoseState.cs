using UnityEngine;

namespace Unity.Game
{
    public class LoseState : UnitState
    {
        public override void Enter()
        {
            _unit.View.StartIdle();
        }
    }
}
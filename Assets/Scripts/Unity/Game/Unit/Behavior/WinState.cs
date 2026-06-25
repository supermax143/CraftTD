using UnityEngine;

namespace Unity.Game
{
    public class WinState : UnitState
    {
        public override void Enter()
        {
            _unit.View.StartIdle();
        }
    }
}
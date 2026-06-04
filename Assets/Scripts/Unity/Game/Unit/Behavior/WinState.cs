using UnityEngine;

namespace Unity.Game
{
    public class WinState : UnitState
    {
        public override void Enter()
        {
            _unit.transform.localScale = Vector3.one * 2;
        }
    }
}
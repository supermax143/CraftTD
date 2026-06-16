using UnityEngine;

namespace Unity.Game
{
    public class LoseState : UnitState
    {
        public override void Enter()
        {
            _unit.transform.localScale = Vector3.one * .5f;
            _unit.View.StartIdle();
        }
    }
}
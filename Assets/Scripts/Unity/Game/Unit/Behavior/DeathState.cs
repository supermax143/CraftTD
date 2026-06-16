namespace Unity.Game
{
    public class DeathState : UnitState
    {
        public override void Enter()
        {
            _unit.Die();
            _unit.View.StartIdle();
        }
    }
}
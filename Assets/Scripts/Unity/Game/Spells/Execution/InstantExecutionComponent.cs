namespace Unity.Game.Spells.Execution
{
    public class InstantExecutionComponent : SpellExecutionComponent
    {
        public override void Execute(SpellController spell)
        {
            spell.EffectApplier.Apply(spell);
            Complete();
        }
    }
}
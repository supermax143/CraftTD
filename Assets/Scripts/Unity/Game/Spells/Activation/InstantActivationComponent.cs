namespace Core.Application.Spells.Activation
{
    public class InstantActivationComponent : SpellActivationComponent
    {
        public override void StartActivationCheck()
        {
            IsActivationCheckActive = false;
        }
    }
}

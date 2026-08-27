namespace Core.Application.Spells.Activation
{
    public class TriggerActivationComponent : SpellActivationComponent
    {
        public override void StartActivationCheck()
        {
            IsActivationCheckActive = true;
        }

        public override void StopActivationCheck()
        {
            IsActivationCheckActive = false;
        }
    }
}

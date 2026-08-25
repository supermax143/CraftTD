namespace Core.Application.Spells.Effects
{
    public abstract class SpellEffect
    {
        public float Duration;
        public float TickInterval;

        public abstract void Apply();
        public virtual void Update() { }
        public virtual void Expire() { }
        public virtual void Remove() { }
    }
}

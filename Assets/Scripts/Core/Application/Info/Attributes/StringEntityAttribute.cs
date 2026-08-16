namespace Unity.Game.Attributes
{
    public abstract class StringEntityAttribute : GameEntityAttribute<string>
    {
        public StringEntityAttribute(string baseValue, GameEntityAttributeKind kind)
            : base(baseValue)
        {
        }
    }
}
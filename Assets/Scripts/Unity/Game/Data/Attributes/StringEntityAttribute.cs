namespace Unity.Game.Attributes
{
    public abstract class StringEntityAttribute : GameEntityAttribute<string>
    {
        public StringEntityAttribute(string value, GameEntityAttributeKind kind) 
            : base(value, kind) { }
    }
}
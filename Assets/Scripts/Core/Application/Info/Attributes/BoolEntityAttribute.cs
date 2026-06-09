namespace Unity.Game.Attributes
{
    public abstract class BoolEntityAttribute : GameEntityAttribute<bool>
    {
        public BoolEntityAttribute(bool value, GameEntityAttributeKind kind) 
            : base(value, kind) { }
    }
}
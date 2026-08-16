namespace Unity.Game.Attributes
{
    public abstract class BoolEntityAttribute : GameEntityAttribute<bool>
    {
        public BoolEntityAttribute(bool baseValue, GameEntityAttributeKind kind)
            : base(baseValue)
        {
        }
    }
}
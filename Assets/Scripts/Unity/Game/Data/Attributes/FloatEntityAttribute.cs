namespace Unity.Game.Attributes
{
    public abstract class FloatEntityAttribute : GameEntityAttribute<float>
    {
        public FloatEntityAttribute(float value, GameEntityAttributeKind kind) 
            : base(value, kind) { }
    }
}
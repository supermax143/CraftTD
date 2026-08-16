namespace Unity.Game.Attributes
{
    public abstract class FloatEntityAttribute : GameEntityAttribute<float>
    {
        public FloatEntityAttribute(float baseValue)
            : base(baseValue)
        {
        }
    }
}
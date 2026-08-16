using System;

namespace Unity.Game.Attributes
{
    [Serializable]
    public abstract class IntEntityAttribute : GameEntityAttribute<int>
    {
        public IntEntityAttribute(int baseValue)
            : base(baseValue)
        {
        }
    }
}
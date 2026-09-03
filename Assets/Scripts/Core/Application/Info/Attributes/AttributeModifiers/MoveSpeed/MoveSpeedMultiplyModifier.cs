using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class MoveSpeedMultiplyModifier : AttributeModifier<float>
    {
        public MoveSpeedMultiplyModifier(int id, float value) : base(value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.MoveSpeed;
        public override ModifierAttributeKind ModifierKind => ModifierAttributeKind.Multiply;

        public override float Apply(float baseValue)
        {
            return baseValue * _value;
        }
    }
}

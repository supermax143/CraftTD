using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class AttackRangeMultiplyModifier : AttributeModifier<float>
    {
        public AttackRangeMultiplyModifier(int id, float value) : base(value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.AttackRange;

        public override ModifierAttributeKind ModifierKind => ModifierAttributeKind.Multiply;

        public override float Apply(float baseValue)
        {
            return baseValue * _value;
        }
    }
}

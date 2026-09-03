using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class DamageMultiplyModifier : AttributeModifier<float>
    {
        public DamageMultiplyModifier(int id, float value) : base(value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.Damage;
        public override ModifierAttributeKind ModifierKind => ModifierAttributeKind.Multiply;

        public override float Apply(float baseValue)
        {
            return baseValue * _value;
        }
    }
}

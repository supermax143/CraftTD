using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class DamageAddModifier : AttributeModifier<float>
    {
        public DamageAddModifier(int id, float value) : base(value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.Damage;
        public override ModifierAttributeKind ModifierKind => ModifierAttributeKind.Add;

        public override float Apply(float baseValue)
        {
            return baseValue + _value;
        }
    }
}

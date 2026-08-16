using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class AttackRangeOverrideModifier : AttributeModifier<float>
    {
        public AttackRangeOverrideModifier(int id, float value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.AttackRange;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Override;

        public override float Apply(float baseValue)
        {
            return _value;
        }
    }
}

using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class AttackRangeAddModifier : AttributeModifier<float>
    {
        public AttackRangeAddModifier(int id, float value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.AttackRange;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Add;

        public override float Apply(float baseValue)
        {
            return baseValue + _value;
        }
    }
}

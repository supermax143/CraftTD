using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class AttackSpeedMultiplyModifier : AttributeModifier<float>
    {
        public AttackSpeedMultiplyModifier(int id, float value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.AttackTime;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Multiply;

        public override float Apply(float baseValue)
        {
            return baseValue * _value;
        }
    }
}

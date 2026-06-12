using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class AttackSpeedOverrideModifier : AttributeModifier<float>
    {
        public AttackSpeedOverrideModifier(int id, float value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.AttackTime;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Override;

        public override float Apply(float baseValue)
        {
            return _value;
        }
    }
}

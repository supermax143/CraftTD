using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class DamageOverrideModifier : AttributeModifier<float>
    {
        public DamageOverrideModifier(int id, float value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.Damage;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Override;

        public override float Apply(float baseValue)
        {
            return _value;
        }
    }
}

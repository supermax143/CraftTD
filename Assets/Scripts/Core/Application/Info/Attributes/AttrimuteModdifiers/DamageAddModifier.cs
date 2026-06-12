using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class DamageAddModifier : AttributeModifier<float>
    {
        public DamageAddModifier(int id, float value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.Damage;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Add;

        public override float Apply(float baseValue)
        {
            return baseValue + _value;
        }
    }
}

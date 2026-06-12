using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class HealthMultiplyModifier : AttributeModifier<int>
    {
        public HealthMultiplyModifier(int id, int value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.Health;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Multiply;

        public override int Apply(int baseValue)
        {
            return baseValue * _value;
        }
    }
}

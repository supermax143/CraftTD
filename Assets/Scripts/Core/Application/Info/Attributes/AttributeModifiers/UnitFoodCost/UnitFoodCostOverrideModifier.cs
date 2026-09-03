using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class UnitFoodCostOverrideModifier : AttributeModifier<int>
    {
        public UnitFoodCostOverrideModifier(int id, int value) : base(value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.UnitFoodCost;
        public override ModifierAttributeKind ModifierKind => ModifierAttributeKind.Override;

        public override int Apply(int baseValue)
        {
            return _value;
        }
    }
}

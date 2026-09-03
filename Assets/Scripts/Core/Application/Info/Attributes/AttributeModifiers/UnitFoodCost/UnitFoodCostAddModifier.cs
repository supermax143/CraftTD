using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class UnitFoodCostAddModifier : AttributeModifier<int>
    {
        public UnitFoodCostAddModifier(int id, int value) : base(value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.UnitFoodCost;
        public override ModifierAttributeKind ModifierKind => ModifierAttributeKind.Add;

        public override int Apply(int baseValue)
        {
            return baseValue + _value;
        }
    }
}

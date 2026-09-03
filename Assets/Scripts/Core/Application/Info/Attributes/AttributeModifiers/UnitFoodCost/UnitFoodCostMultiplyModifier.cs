using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class UnitFoodCostMultiplyModifier : AttributeModifier<int>
    {
        public UnitFoodCostMultiplyModifier(int id, int value) : base(value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.UnitFoodCost;
        public override ModifierAttributeKind ModifierKind => ModifierAttributeKind.Multiply;

        public override int Apply(int baseValue)
        {
            return baseValue * _value;
        }
    }
}

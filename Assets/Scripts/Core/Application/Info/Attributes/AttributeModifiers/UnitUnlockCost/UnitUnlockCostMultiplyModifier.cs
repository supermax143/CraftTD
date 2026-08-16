using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class UnitUnlockCostMultiplyModifier : AttributeModifier<int>
    {
        public UnitUnlockCostMultiplyModifier(int id, int value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.UnitUnlockCost;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Multiply;

        public override int Apply(int baseValue)
        {
            return baseValue * _value;
        }
    }
}

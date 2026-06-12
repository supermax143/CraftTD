using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class UnitUnlockCostAddModifier : AttributeModifier<int>
    {
        public UnitUnlockCostAddModifier(int id, int value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.UnitUnlockCost;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Add;

        public override int Apply(int baseValue)
        {
            return baseValue + _value;
        }
    }
}

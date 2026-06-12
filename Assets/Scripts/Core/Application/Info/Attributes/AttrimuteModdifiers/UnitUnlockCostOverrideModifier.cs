using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class UnitUnlockCostOverrideModifier : AttributeModifier<int>
    {
        public UnitUnlockCostOverrideModifier(int id, int value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.UnitUnlockCost;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Override;

        public override int Apply(int baseValue)
        {
            return _value;
        }
    }
}

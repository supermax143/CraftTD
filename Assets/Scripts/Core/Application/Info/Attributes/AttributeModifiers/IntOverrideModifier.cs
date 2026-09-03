using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class IntOverrideModifier<T> : AttributeModifier<int> where T : IntEntityAttribute
    {
        private readonly GameEntityAttributeKind _attributeKind;

        public IntOverrideModifier(int value) : base(value)
        {
            _value = value;
            _attributeKind = GetAttributeKind<T>();
        }

        public override GameEntityAttributeKind AttributeKind => _attributeKind;
        public override ModifierAttributeKind ModifierKind => ModifierAttributeKind.Override;

        public override int Apply(int baseValue)
        {
            return _value;
        }
        
        private GameEntityAttributeKind GetAttributeKind<TAttribute>() where TAttribute : IntEntityAttribute
        {
            if (typeof(TAttribute) == typeof(HealthAttribute))
                return GameEntityAttributeKind.Health;
            if (typeof(TAttribute) == typeof(UnitFoodCostAttribute))
                return GameEntityAttributeKind.UnitFoodCost;
            if (typeof(TAttribute) == typeof(UnitUnlockCostAttribute))
                return GameEntityAttributeKind.UnitUnlockCost;
            if (typeof(TAttribute) == typeof(RewardMoneyAttribute))
                return GameEntityAttributeKind.RewardMoney;
            
            return GameEntityAttributeKind.Health;
        }
    }
}

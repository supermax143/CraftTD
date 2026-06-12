using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class IntAddModifier<T> : AttributeModifier<int> where T : IntEntityAttribute
    {
        private readonly GameEntityAttributeKind _attributeKind;

        public IntAddModifier(int id, int value) : base(id, value)
        {
            _value = value;
            _attributeKind = GetAttributeKind<T>();
        }

        public override GameEntityAttributeKind AttributeKind => _attributeKind;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Add;

        public override int Apply(int baseValue)
        {
            return baseValue + _value;
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

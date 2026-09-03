using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class RewardMoneyMultiplyModifier : AttributeModifier<int>
    {
        public RewardMoneyMultiplyModifier(int id, int value) : base(value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.RewardMoney;
        public override ModifierAttributeKind ModifierKind => ModifierAttributeKind.Multiply;

        public override int Apply(int baseValue)
        {
            return baseValue * _value;
        }
    }
}

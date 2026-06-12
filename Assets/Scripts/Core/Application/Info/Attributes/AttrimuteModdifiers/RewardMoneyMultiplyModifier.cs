using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class RewardMoneyMultiplyModifier : AttributeModifier<int>
    {
        public RewardMoneyMultiplyModifier(int id, int value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.RewardMoney;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Multiply;

        public override int Apply(int baseValue)
        {
            return baseValue * _value;
        }
    }
}

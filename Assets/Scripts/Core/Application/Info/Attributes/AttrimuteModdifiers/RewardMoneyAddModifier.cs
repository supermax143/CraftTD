using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class RewardMoneyAddModifier : AttributeModifier<int>
    {
        public RewardMoneyAddModifier(int id, int value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.RewardMoney;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Add;

        public override int Apply(int baseValue)
        {
            return baseValue + _value;
        }
    }
}

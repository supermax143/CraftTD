using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class HealthTowerAddModifier : AttributeModifier<int>
    {
        private readonly int _value;

        public HealthTowerAddModifier(int value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.Health;

        public override int Apply(int baseValue)
        {
            return baseValue += _value;
        }
    }
}
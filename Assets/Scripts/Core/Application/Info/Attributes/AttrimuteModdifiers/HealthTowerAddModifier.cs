using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class HealthTowerAddModifier : AttributeModifier<int>
    {
        public HealthTowerAddModifier(int id, int value) : base(id, value)
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
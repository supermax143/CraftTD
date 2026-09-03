using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class HealthAddModifier : AttributeModifier<int>
    {
        public HealthAddModifier(int id, int value) : base(value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.Health;
        public override ModifierAttributeKind ModifierKind => ModifierAttributeKind.Add;
        
        public override int Apply(int baseValue)
        {
            return baseValue += _value;
        }
    }
}
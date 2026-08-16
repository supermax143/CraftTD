using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class DetectionRangeOverrideModifier : AttributeModifier<float>
    {
        public DetectionRangeOverrideModifier(int id, float value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.DetectionRange;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Override;

        public override float Apply(float baseValue)
        {
            return _value;
        }
    }
}

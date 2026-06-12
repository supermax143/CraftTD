using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttrimuteModdifiers
{
    public class MoveSpeedAddModifier : AttributeModifier<float>
    {
        public MoveSpeedAddModifier(int id, float value) : base(id, value)
        {
            _value = value;
        }

        public override GameEntityAttributeKind AttributeKind => GameEntityAttributeKind.MoveSpeed;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Add;

        public override float Apply(float baseValue)
        {
            return baseValue + _value;
        }
    }
}

using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    public class FloatAddModifier<T> : AttributeModifier<float> where T : FloatEntityAttribute
    {
        private readonly GameEntityAttributeKind _attributeKind;

        public FloatAddModifier(float value) : base(value)
        {
            _value = value;
            _attributeKind = GetAttributeKind<T>();
        }

        public override GameEntityAttributeKind AttributeKind => _attributeKind;
        public ModifierAttributeKind ModifierKind => ModifierAttributeKind.Add;

        public override float Apply(float baseValue)
        {
            return baseValue + _value;
        }
        
        private GameEntityAttributeKind GetAttributeKind<TAttribute>() where TAttribute : FloatEntityAttribute
        {
            if (typeof(TAttribute) == typeof(DamageAttribute))
                return GameEntityAttributeKind.Damage;
            if (typeof(TAttribute) == typeof(AttackRangeAttribute))
                return GameEntityAttributeKind.AttackRange;
            if (typeof(TAttribute) == typeof(MoveSpeedAttribute))
                return GameEntityAttributeKind.MoveSpeed;
            if (typeof(TAttribute) == typeof(AttackSpeedAttribute))
                return GameEntityAttributeKind.AttackTime;
            if (typeof(TAttribute) == typeof(DetectionRangeAttribute))
                return GameEntityAttributeKind.DetectionRange;
            
            return GameEntityAttributeKind.Damage;
        }
    }
}

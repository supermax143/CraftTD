using System;
using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    [Serializable]
    public class AttributeModifierWrapper
    {
        [SerializeField]
        private GameEntityAttributeKind _attributeKind;
        [SerializeField]
        private ModifierAttributeKind _modifierKind;
        
        [SerializeField]
        private float _value;
        
        public string Label => $"{_attributeKind}_{_modifierKind}_{_value}";
        
        
        public AttributeModifierBase GetModifier()
        {
            return _attributeKind switch
            {
                GameEntityAttributeKind.Damage => CreateFloatModifier<DamageAttribute>(_value, _modifierKind),
                GameEntityAttributeKind.AttackRange => CreateFloatModifier<AttackRangeAttribute>(_value, _modifierKind),
                GameEntityAttributeKind.MoveSpeed => CreateFloatModifier<MoveSpeedAttribute>(_value, _modifierKind),
                GameEntityAttributeKind.AttackTime => CreateFloatModifier<AttackSpeedAttribute>(_value, _modifierKind),
                GameEntityAttributeKind.DetectionRange => CreateFloatModifier<DetectionRangeAttribute>(_value, _modifierKind),
                GameEntityAttributeKind.Health => CreateIntModifier<HealthAttribute>(_value, _modifierKind),
                GameEntityAttributeKind.UnitFoodCost => CreateIntModifier<UnitFoodCostAttribute>(_value, _modifierKind),
                GameEntityAttributeKind.UnitUnlockCost => CreateIntModifier<UnitUnlockCostAttribute>(_value, _modifierKind),
                GameEntityAttributeKind.RewardMoney => CreateIntModifier<RewardMoneyAttribute>(_value, _modifierKind),
                _ => null
            };
        }
        
        private AttributeModifierBase CreateFloatModifier<T>(float value, ModifierAttributeKind kind) where T : FloatEntityAttribute
        {
            return kind switch
            {
                ModifierAttributeKind.Add => new FloatAddModifier<T>(value),
                ModifierAttributeKind.Multiply => new FloatMultiplyModifier<T>(value),
                ModifierAttributeKind.Override => new FloatOverrideModifier<T>(value),
                _ => null
            };
        }
        
        private AttributeModifierBase CreateIntModifier<T>(float value, ModifierAttributeKind kind) where T : IntEntityAttribute
        {
            int intValue = Mathf.RoundToInt(value);
            return kind switch
            {
                ModifierAttributeKind.Add => new IntAddModifier<T>(intValue),
                ModifierAttributeKind.Multiply => new IntMultiplyModifier<T>(intValue),
                ModifierAttributeKind.Override => new IntOverrideModifier<T>(intValue),
                _ => null
            };
        }
    }
}

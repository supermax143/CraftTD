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
        
        
        public AttributeModifierBase GetModifier(int id)
        {
            return _attributeKind switch
            {
                GameEntityAttributeKind.Damage => CreateFloatModifier<DamageAttribute>(id, _value, _modifierKind),
                GameEntityAttributeKind.AttackRange => CreateFloatModifier<AttackRangeAttribute>(id, _value, _modifierKind),
                GameEntityAttributeKind.MoveSpeed => CreateFloatModifier<MoveSpeedAttribute>(id, _value, _modifierKind),
                GameEntityAttributeKind.AttackTime => CreateFloatModifier<AttackSpeedAttribute>(id, _value, _modifierKind),
                GameEntityAttributeKind.DetectionRange => CreateFloatModifier<DetectionRangeAttribute>(id, _value, _modifierKind),
                GameEntityAttributeKind.Health => CreateIntModifier<HealthAttribute>(id, _value, _modifierKind),
                GameEntityAttributeKind.UnitFoodCost => CreateIntModifier<UnitFoodCostAttribute>(id, _value, _modifierKind),
                GameEntityAttributeKind.UnitUnlockCost => CreateIntModifier<UnitUnlockCostAttribute>(id, _value, _modifierKind),
                GameEntityAttributeKind.RewardMoney => CreateIntModifier<RewardMoneyAttribute>(id, _value, _modifierKind),
                _ => null
            };
        }
        
        private AttributeModifierBase CreateFloatModifier<T>(int id, float value, ModifierAttributeKind kind) where T : FloatEntityAttribute
        {
            return kind switch
            {
                ModifierAttributeKind.Add => new FloatAddModifier<T>(id, value),
                ModifierAttributeKind.Multiply => new FloatMultiplyModifier<T>(id, value),
                ModifierAttributeKind.Override => new FloatOverrideModifier<T>(id, value),
                _ => null
            };
        }
        
        private AttributeModifierBase CreateIntModifier<T>(int id, float value, ModifierAttributeKind kind) where T : IntEntityAttribute
        {
            int intValue = Mathf.RoundToInt(value);
            return kind switch
            {
                ModifierAttributeKind.Add => new IntAddModifier<T>(id, intValue),
                ModifierAttributeKind.Multiply => new IntMultiplyModifier<T>(id, intValue),
                ModifierAttributeKind.Override => new IntOverrideModifier<T>(id, intValue),
                _ => null
            };
        }
    }
}

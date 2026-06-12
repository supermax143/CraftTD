using System;
using Sirenix.OdinInspector;
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
        private float _floatValue;
        
        [SerializeField]
        private int _intValue;
        
        [ShowInInspector]
        public GameEntityAttributeKind AttributeKind => _attributeKind;
        
        [ShowInInspector]
        public ModifierAttributeKind ModifierKind => _modifierKind;
        
        public AttributeModifierBase GetModifier(int id)
        {
            return _attributeKind switch
            {
                GameEntityAttributeKind.Damage => CreateFloatModifier<DamageAttribute>(id, _floatValue, _modifierKind),
                GameEntityAttributeKind.AttackRange => CreateFloatModifier<AttackRangeAttribute>(id, _floatValue, _modifierKind),
                GameEntityAttributeKind.MoveSpeed => CreateFloatModifier<MoveSpeedAttribute>(id, _floatValue, _modifierKind),
                GameEntityAttributeKind.AttackTime => CreateFloatModifier<AttackSpeedAttribute>(id, _floatValue, _modifierKind),
                GameEntityAttributeKind.DetectionRange => CreateFloatModifier<DetectionRangeAttribute>(id, _floatValue, _modifierKind),
                GameEntityAttributeKind.Health => CreateIntModifier<HealthAttribute>(id, _intValue, _modifierKind),
                GameEntityAttributeKind.UnitFoodCost => CreateIntModifier<UnitFoodCostAttribute>(id, _intValue, _modifierKind),
                GameEntityAttributeKind.UnitUnlockCost => CreateIntModifier<UnitUnlockCostAttribute>(id, _intValue, _modifierKind),
                GameEntityAttributeKind.RewardMoney => CreateIntModifier<RewardMoneyAttribute>(id, _intValue, _modifierKind),
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
        
        private AttributeModifierBase CreateIntModifier<T>(int id, int value, ModifierAttributeKind kind) where T : IntEntityAttribute
        {
            return kind switch
            {
                ModifierAttributeKind.Add => new IntAddModifier<T>(id, value),
                ModifierAttributeKind.Multiply => new IntMultiplyModifier<T>(id, value),
                ModifierAttributeKind.Override => new IntOverrideModifier<T>(id, value),
                _ => null
            };
        }
    }
}

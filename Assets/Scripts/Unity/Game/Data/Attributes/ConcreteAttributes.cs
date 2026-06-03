using System;
using Unity.Game.Data.Attributes;
using UnityEngine;

namespace Unity.Game.Attributes.Specific
{
    [Serializable]
    public class DamageAttribute  : FloatEntityAttribute
    {
        public DamageAttribute (float value = 0f) 
            : base(value, GameEntityAttributeKind.Damage) { }
    }
    
    [Serializable]
    public class AttackRangeAttribute : FloatEntityAttribute
    {
        public AttackRangeAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.AttackRange) { }
    }
    
    [Serializable]
    public class MoveSpeedAttribute : FloatEntityAttribute
    {
        public MoveSpeedAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.MoveSpeed) { }
    }
    
    [Serializable]
    public class AttackSpeedAttribute : FloatEntityAttribute
    {
        public AttackSpeedAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.AttackTime) { }
    }
    
    [Serializable]
    public class AttackCooldownAttribute : FloatEntityAttribute
    {
        public AttackCooldownAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.AttackCooldown) { }
    }
    
    [Serializable]
    public class DetectionRangeAttribute : FloatEntityAttribute
    {
        public DetectionRangeAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.DetectionRange) { }
    }
    
    [Serializable]
    public class DetectionIntervalAttribute : FloatEntityAttribute
    {
        public DetectionIntervalAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.DetectionInterval) { }
    }
    
    [Serializable]
    public class HealthAttribute : IntEntityAttribute
    {
        public HealthAttribute(int value = 0) 
            : base(value, GameEntityAttributeKind.Health) { }
    }
    
    [Serializable]
    public class UnitCostAttribute : IntEntityAttribute
    {
        public UnitCostAttribute(int value = 0) 
            : base(value, GameEntityAttributeKind.UnitCost) { }
    }
    
    
    [Serializable]
    public class UnitPrefabAttribute : GameObjectEntityAttribute
    {
        public UnitPrefabAttribute(GameObject value = null) 
            : base(value, GameEntityAttributeKind.UnitCost) { }
    }
    
}
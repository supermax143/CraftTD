using System;

namespace Unity.Game.Attributes.Specific
{
    public class DamageAttribute  : FloatEntityAttribute
    {
        public DamageAttribute (float value = 0f) 
            : base(value, GameEntityAttributeKind.Damage) { }
    }
    
    public class AttackRangeAttribute : FloatEntityAttribute
    {
        public AttackRangeAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.AttackRange) { }
    }
    
    public class MoveSpeedAttribute : FloatEntityAttribute
    {
        public MoveSpeedAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.MoveSpeed) { }
    }
    
    public class AttackTimeAttribute : FloatEntityAttribute
    {
        public AttackTimeAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.AttackTime) { }
    }
    
    public class AttackCooldownAttribute : FloatEntityAttribute
    {
        public AttackCooldownAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.AttackCooldown) { }
    }
    
    public class DetectionRangeAttribute : FloatEntityAttribute
    {
        public DetectionRangeAttribute(float value = 0f) 
            : base(value, GameEntityAttributeKind.DetectionRange) { }
    }
    
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
    public class NameAttribute : StringEntityAttribute
    {
        public NameAttribute(string value = "") 
            : base(value, GameEntityAttributeKind.Name) { }
    }
    
}
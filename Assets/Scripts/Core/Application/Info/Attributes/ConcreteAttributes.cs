using System;
using Unity.Game.Data.Attributes;
using UnityEngine;

namespace Unity.Game.Attributes.Specific
{
    [Serializable]
    public class DamageAttribute : FloatEntityAttribute
    {
        public DamageAttribute(float value = 0f)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.Damage;
    }

    [Serializable]
    public class AttackRangeAttribute : FloatEntityAttribute
    {
        public AttackRangeAttribute(float value = 0f)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.AttackRange;
    }

    [Serializable]
    public class MoveSpeedAttribute : FloatEntityAttribute
    {
        public MoveSpeedAttribute(float value = 0f)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.MoveSpeed;
    }

    [Serializable]
    public class AttackSpeedAttribute : FloatEntityAttribute
    {
        public AttackSpeedAttribute(float value = 0f)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.AttackTime;
    }


    [Serializable]
    public class DetectionRangeAttribute : FloatEntityAttribute
    {
        public DetectionRangeAttribute(float value = 0f)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.DetectionRange;
    }

    
    [Serializable]
    public class HealthAttribute : IntEntityAttribute
    {
        public HealthAttribute(int value = 0)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.Health;
    }

    [Serializable]
    public class UnitFoodCostAttribute : IntEntityAttribute
    {
        public UnitFoodCostAttribute(int value = 0)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.UnitFoodCost;
    }


    [Serializable]
    public class UnitUnlockCostAttribute : IntEntityAttribute
    {
        public UnitUnlockCostAttribute(int value = 0)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.UnitUnlockCost;
    }

    [Serializable]
    public class RewardMoneyAttribute : IntEntityAttribute
    {
        public RewardMoneyAttribute(int value = 0)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.RewardMoney;
    }

    [Serializable]
    public class TowerPrefabAttribute : GameObjectEntityAttribute
    {
        public TowerPrefabAttribute(GameObject value = null)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.TowerPrefab;
    }

    [Serializable]
    public class UnitPrefabAttribute : GameObjectEntityAttribute
    {
        public UnitPrefabAttribute(GameObject value = null)
            : base(value)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.UnitPrefab;
    }
}
using System;
using Unity.Game.Data.Attributes;
using UnityEngine;

namespace Unity.Game.Attributes.Specific
{
    [Serializable]
    public class DamageAttribute : FloatEntityAttribute
    {
        public DamageAttribute(float baseValue = 0f)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.Damage;
    }

    [Serializable]
    public class AttackRangeAttribute : FloatEntityAttribute
    {
        public AttackRangeAttribute(float baseValue = 0f)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.AttackRange;
    }

    [Serializable]
    public class MoveSpeedAttribute : FloatEntityAttribute
    {
        public MoveSpeedAttribute(float baseValue = 0f)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.MoveSpeed;
    }

    [Serializable]
    public class AttackSpeedAttribute : FloatEntityAttribute
    {
        public AttackSpeedAttribute(float baseValue = 0f)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.AttackTime;
    }


    [Serializable]
    public class DetectionRangeAttribute : FloatEntityAttribute
    {
        public DetectionRangeAttribute(float baseValue = 0f)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.DetectionRange;
    }

    
    [Serializable]
    public class HealthAttribute : IntEntityAttribute
    {
        public HealthAttribute(int baseValue = 0)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.Health;
    }

    [Serializable]
    public class UnitFoodCostAttribute : IntEntityAttribute
    {
        public UnitFoodCostAttribute(int baseValue = 0)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.UnitFoodCost;
    }


    [Serializable]
    public class UnitUnlockCostAttribute : IntEntityAttribute
    {
        public UnitUnlockCostAttribute(int baseValue = 0)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.UnitUnlockCost;
    }

    [Serializable]
    public class RewardMoneyAttribute : IntEntityAttribute
    {
        public RewardMoneyAttribute(int baseValue = 0)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.RewardMoney;
    }

    [Serializable]
    public class DefenseStanceCostAttribute : IntEntityAttribute
    {
        public DefenseStanceCostAttribute(int baseValue = 0)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.DefenseStanceCost;
    }

    [Serializable]
    public class TowerPrefabAttribute : GameObjectEntityAttribute
    {
        public TowerPrefabAttribute(GameObject baseValue = null)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.TowerPrefab;
    }

    [Serializable]
    public class UnitPrefabAttribute : GameObjectEntityAttribute
    {
        public UnitPrefabAttribute(GameObject baseValue = null)
            : base(baseValue)
        {
        }

        public override GameEntityAttributeKind Kind => GameEntityAttributeKind.UnitPrefab;
    }
}
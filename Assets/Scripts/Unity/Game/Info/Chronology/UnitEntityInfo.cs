using System;
using Core.Application.Interfaces.Info;
using Sirenix.OdinInspector;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{

    [Serializable]
    public class UnitEntityInfo : GameEntityInfo, IUnitInfo
    {
        [SerializeField] 
        private string _name;
        [SerializeField] 
        private UnitTier _tier;

        
        [InlineProperty, SerializeField] 
        private HealthAttribute _health;
        [InlineProperty, SerializeField] 
        private MoveSpeedAttribute _moveSpeed;
        [InlineProperty, SerializeField] 
        private DamageAttribute _damage;
        [InlineProperty, SerializeField] 
        private AttackRangeAttribute _attackRange;
        [InlineProperty, SerializeField] 
        private AttackSpeedAttribute _attackSpeed;
        [InlineProperty, SerializeField] 
        private AttackCooldownAttribute _attackCooldown;
        [InlineProperty, SerializeField] 
        private DetectionRangeAttribute _detectionRange;
        [InlineProperty, SerializeField] 
        private UnitFoodCostAttribute unitFoodCost;
        [InlineProperty, SerializeField] 
        private UnitUnlockCostAttribute unitUnlockCost;
        [InlineProperty, SerializeField] 
        private RewardMoneyAttribute _rewardMoney;
        [InlineProperty, SerializeField] 
        private UnitPrefabAttribute _unitPrefab;
        
        public string Name => _name;
        public UnitTier Tier => _tier;
        public int FoodCost => unitFoodCost.Value;
        public int UnlockCost => unitUnlockCost.Value;
    }
}

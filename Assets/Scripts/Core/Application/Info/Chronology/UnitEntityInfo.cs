using System;
using Sirenix.OdinInspector;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{

    [Serializable]
    public class UnitEntityInfo : GameEntityInfo
    {
        [SerializeField] 
        private string _name;
        [SerializeField] 
        private UnitTier _tier;

        
        
        [InlineProperty, SerializeField] 
        private UnitPrefabAttribute _unitPrefab;
        
        [InlineProperty, SerializeField] 
        private MoveSpeedAttribute _moveSpeed;
        [InlineProperty, SerializeField] 
        private AttackRangeAttribute _attackRange;
        [InlineProperty, SerializeField] 
        private AttackSpeedAttribute _attackSpeed;
        
        private HealthAttribute _health;
        private DamageAttribute _damage;
        private UnitFoodCostAttribute _foodCost;
        private UnitUnlockCostAttribute _unlockCost = new();
        private RewardMoneyAttribute _rewardMoney = new();
        
        public string Name => _name;
        public UnitTier Tier => _tier;
        public int FoodCost => _foodCost.ValueModified;
        public int UnlockCost => _unlockCost.ValueModified;

        public void Initialize(GameStats gameStats, uint epoch)
        {
            _unlockCost = 
                new UnitUnlockCostAttribute(gameStats.GetUnitOpeningCost(epoch, _tier));
            _rewardMoney = 
                new RewardMoneyAttribute(gameStats.GetUnitKillReward(epoch, _tier));
            _health = 
                new HealthAttribute(gameStats.GetUnitHealth(_tier, (int)epoch));
            _damage = 
                new DamageAttribute(gameStats.GetUnitDamage(_tier, (int)epoch));
            _foodCost = 
                new UnitFoodCostAttribute(gameStats.GetUnitFood(_tier, (int)epoch));
        }
    }
}

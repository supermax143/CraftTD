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
        private HealthAttribute _health;
        [InlineProperty, SerializeField] 
        private DamageAttribute _damage;
        [InlineProperty, SerializeField] 
        private UnitPrefabAttribute _unitPrefab;
        
        [InlineProperty, SerializeField] 
        private UnitFoodCostAttribute _foodCost;
        [InlineProperty, SerializeField] 
        private MoveSpeedAttribute _moveSpeed;
        [InlineProperty, SerializeField] 
        private AttackRangeAttribute _attackRange;
        [InlineProperty, SerializeField] 
        private AttackSpeedAttribute _attackSpeed;
        
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
            
        }
    }
}

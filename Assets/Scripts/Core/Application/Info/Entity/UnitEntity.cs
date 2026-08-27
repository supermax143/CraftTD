using Unity.Game.Attributes.Specific;

namespace Unity.Game.Entity
{
    public class UnitEntity : GameEntityData
    {
        private MoveSpeedAttribute _moveSpeed;
        private AttackRangeAttribute _attackRange;
        private AttackSpeedAttribute _attackSpeed;
        private HealthAttribute _health;
        private DamageAttribute _damage;
        private UnitFoodCostAttribute _foodCost;
        private UnitUnlockCostAttribute _unlockCost;
        private RewardMoneyAttribute _rewardMoney;
       
        private readonly Faction _faction;

        public int FoodCost => _foodCost.BaseBaseValueModified;
        public int UnlockCost => _unlockCost.BaseBaseValueModified;
        public Faction Faction => _faction;

        public UnitEntity(UnitTier _tier, Faction faction, uint epoch, GameStats gameStats)
        {
            _faction = faction;
            
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
            _moveSpeed = 
                new MoveSpeedAttribute(gameStats.GetUnitMoveSpeed(_tier, (int)epoch));
            _attackRange = 
                new AttackRangeAttribute(gameStats.GetUnitAttackRange(_tier, (int)epoch));
            _attackSpeed = 
                new AttackSpeedAttribute(gameStats.GetUnitAttackSpeed(_tier, (int)epoch));
        }
        
    }
}
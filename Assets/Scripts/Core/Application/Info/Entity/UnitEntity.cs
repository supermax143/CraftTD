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
        private DefenseStanceCostAttribute _defenseStanceCost;

        private readonly Faction _faction;
        private readonly UnitTier _tier;

        public int FoodCost => _foodCost.BaseValueModified;
        public int UnlockCost => _unlockCost.BaseValueModified;
        public int DefenseStanceCost => _defenseStanceCost.BaseValueModified;
        public Faction Faction => _faction;
        public UnitTier Tier => _tier;

        public UnitEntity(UnitTier _tier, Faction faction, uint epoch, GameStats gameStats)
        {
            _faction = faction;
            _tier = _tier;

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
            _defenseStanceCost =
                new DefenseStanceCostAttribute(gameStats.GetDefenseStanceCost((int)epoch, _tier));
        }
        
    }
}
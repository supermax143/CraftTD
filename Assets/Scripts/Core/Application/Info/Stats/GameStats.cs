using System;
using UnityEngine;

namespace Unity.Game
{
    
    [CreateAssetMenu(menuName = "CraftTD/GameStats", order = 1)]
    public class GameStats : ScriptableObject
    {

        [Header("Food Production")]
        [SerializeField] private float _baseFoodProductionSpeed;
        [SerializeField] private float _foodProductionPerLevel;
        
        [Space]
        [Header("Tower Upgrade")]
        [SerializeField] private int _baseTowerUpgradeHealth;
        [SerializeField] private int _towerUpgradeHealthPerLevel;
        
        [Space]
        [Header("Epoch Complete")]
        [SerializeField] private int _baseEpochCompleteCost;
        [SerializeField] private int _epochCompleteCostMultiplier;
        
        [Space]
        [Header("Tower Health")]
        [SerializeField] private int _baseTowerHealthPlayer;
        [SerializeField] private int _baseTowerHealthEnamy;
        [SerializeField] private int _towerHealthMultiplier;
        
        
        [Space]
        [Header("Tower Reward")]
        [SerializeField] private int _baseTowerReward;
        [SerializeField] private int _towerRewardMultiplier;
        
        [Space]
        [Header("Unit Opening")]
        [SerializeField] private int _baseUnitOpeningCostTier2;
        [SerializeField] private int _baseUnitOpeningCostTier3;
        [SerializeField] private int _unitOpeningCostMultiplier;
        
        [Space]
        [Header("Unit Reward")]
        [SerializeField] private int _baseUnitRewardTier1;
        [SerializeField] private int _baseUnitRewardTier2;
        [SerializeField] private int _baseUnitRewardTier3;
        [SerializeField] private int _unitRewardMultiplier;
        
        public int GetUnitKillReward(uint epoch, UnitTier tier)
        {
            var baseCost = tier switch
            {
                UnitTier.Tier1 => _baseUnitRewardTier1,
                UnitTier.Tier2 => _baseUnitRewardTier2,
                UnitTier.Tier3 => _baseUnitRewardTier3,
                _ => 0
            };
            return (int)(baseCost * Math.Pow(_unitRewardMultiplier, epoch));
        }
        
        
        public int StartFoodCount(int epoch)
        {
            if (epoch <= 2)
            {
                return epoch;
            }

            return epoch + 1;
        }
        
        public int GetTowerKillReward(uint epoch)
            => (int)(_baseTowerReward * Math.Pow(_towerRewardMultiplier, epoch));
        
        public int GetUnitOpeningCost(uint epoch, UnitTier tier)
        {
            var baseCost = tier == UnitTier.Tier2 ? _baseUnitOpeningCostTier2 : _baseUnitOpeningCostTier3;
            return (int)(baseCost * Math.Pow(_unitOpeningCostMultiplier, epoch));
        }
        
        public float BaseFoodProductionSpeed => _baseFoodProductionSpeed;
        public float FoodProductionPerLevel => _foodProductionPerLevel;
        public float BaseTowerUpgradeHealth => _baseTowerUpgradeHealth;
        public float TowerUpgradeHealthPerLevel => _towerUpgradeHealthPerLevel;
        
        internal int GetFoodProductionSpeedCost(uint level) 
            => (int)Math.Floor( 8f * Math.Pow(1.18, (float)level));
        
        internal float GetFoodProductionSpeed(uint level)
            => _baseFoodProductionSpeed + _foodProductionPerLevel * level;

        internal int GetTowerUpgradeCost(uint towerUpgradeLevel) 
            => (int)Math.Floor( 16f * Math.Pow(1.18, (float)towerUpgradeLevel));

        public int GetTowerHealth(uint towerLevel) 
            => (int)(_baseTowerUpgradeHealth + _towerUpgradeHealthPerLevel * towerLevel);

        public int GetEpochCompleteCost(int epoch) =>
            (int)(_baseEpochCompleteCost * Math.Pow(_epochCompleteCostMultiplier, epoch - 1));

        public int GetTowerHealth(uint epoch, Faction faction)
        {
            var baseHealth = faction == Faction.Player ? _baseTowerHealthPlayer : _baseTowerHealthEnamy;
            return (int)(baseHealth * Math.Pow(_towerHealthMultiplier, epoch - 1));
        }
    }
}
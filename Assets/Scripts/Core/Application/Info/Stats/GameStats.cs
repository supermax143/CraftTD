using System;
using UnityEngine;

namespace Unity.Game
{
    
    [CreateAssetMenu(menuName = "CraftTD/GameStats", order = 1)]
    public class GameStats : ScriptableObject
    {

        [SerializeField] private float _baseFoodProductionSpeed = .18f;
        [SerializeField] private float _foodProductionPerLevel = .02f;
        
        [SerializeField] private int _baseTowerUpgradeHealth = 2;
        [SerializeField] private int _towerUpgradeHealthPerLevel = 2;
        
        [SerializeField] private int _baseEpochCompleteCost = 2000;
        [SerializeField] private int _epochCompleteCostMultiplier = 8;
        
        [SerializeField] private int _baseTowerHealthPlayer = 2;
        [SerializeField] private int _baseTowerHealthEnamy = 500;
        [SerializeField] private int _towerHealthMultiplier = 3;
        
        
        [SerializeField] private int _baseTowerRewardPlayer = 100;
        [SerializeField] private int _towerRewardMultiplier = 2;
        
        public int GetTowerReward(uint epoch, Faction faction)
            => (int)(_baseTowerRewardPlayer * Math.Pow(_towerRewardMultiplier, epoch));

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
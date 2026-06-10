using System;
using UnityEngine;

namespace Unity.Game
{
    
    [CreateAssetMenu(menuName = "CraftTD/GameStats", order = 1)]
    public class GameStats : ScriptableObject
    {

        [SerializeField] private float _baseFoodProductionSpeed = .18f;
        [SerializeField] private float _foodProductionPerLevel = .02f;
        [SerializeField] private float _baseTowerHealth = 2;
        [SerializeField] private float _towerHealthPerLevel = 2;
        [SerializeField] private float _baseEpochCompleteCost = 2000;
        [SerializeField] private float _epochCompleteCostMultiplier = 8;
        
        public float BaseFoodProductionSpeed => _baseFoodProductionSpeed;
        public float FoodProductionPerLevel => _foodProductionPerLevel;
        public float BaseTowerHealth => _baseTowerHealth;
        public float TowerHealthPerLevel => _towerHealthPerLevel;
        
        internal int GetFoodProductionSpeedCost(uint level) 
            => (int)Math.Floor( 8f * Math.Pow(1.18, (float)level));
        
        internal float GetFoodProductionSpeed(uint level)
            => _baseFoodProductionSpeed + _foodProductionPerLevel * level;

        internal int GetTowerUpgradeCost(uint towerUpgradeLevel) 
            => (int)Math.Floor( 16f * Math.Pow(1.18, (float)towerUpgradeLevel));

        public int GetTowerHealth(uint towerLevel) 
            => (int)(_baseTowerHealth + _towerHealthPerLevel * towerLevel);
        
        public int GetEpochCompleteCost(int epochIndex)  => (int)(_baseEpochCompleteCost * epochIndex);
        
    }
}
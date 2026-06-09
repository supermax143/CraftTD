using System;
using UnityEngine;

namespace Unity.Game
{
    
    [CreateAssetMenu(menuName = "CraftTD/GameStats", order = 1)]
    public class GameStats : ScriptableObject
    {

        [SerializeField] private float _baseFoodProductionSpeed = .2f;
        [SerializeField] private float _foodProductionPerLevel = .2f;
        [SerializeField] private float _baseTowerHealth = 2;
        [SerializeField] private float _towerHealthPerLevel = 2;
       
        public float BaseFoodProductionSpeed => _baseFoodProductionSpeed;
        public float FoodProductionPerLevel => _foodProductionPerLevel;
        public float BaseTowerHealth => _baseTowerHealth;
        public float TowerHealthPerLevel => _towerHealthPerLevel;
        
        
        public static float FoodProductionSpeedCost(int level) => (float)Math.Floor( 8f * Math.Pow(1.18, (float)level));
        
    }
}
using System;
using System.Collections.Generic;
using Core.Application.DataStorage.StorageItems;
using Unity.Game;

namespace Core.Application.Models
{
    public class EpochModel
    {
        public event Action OnUnitOpened;
        public event Action OnMoneyChanged;
        public event Action OnFoodProductionLevelChanged;
        public event Action OnTowerLevelChanged;
        
        private readonly List<UnitModel> _units = new();
        private readonly EpochInfo _info;
        private readonly EpochStorageData _data;
        private readonly GameStats _gameStats;
        
        public uint Money
        {
            get => _data.Money;
            set => _data.Money = value;
        }

        public uint FoodProductionLevel => _data.FoodProductionLevel;
        public uint TowerLevel => _data.TowerLevel;
        public IEnumerable<UnitModel> Units => _units;
        public float FoodProductionSpeed => _gameStats.GetFoodProductionSpeed(FoodProductionLevel);
        public int FoodProductionUpgradeCost => _gameStats.GetFoodProductionSpeedCost(FoodProductionLevel);
        public int TowerUpgradeCost => _gameStats.GetTowerUpgradeCost(TowerLevel);
        public object TowerHealth => _gameStats.GetTowerHealth(TowerLevel);

        internal EpochModel(EpochInfo info, EpochStorageData data, GameStats gameStats)
        {
            _info = info;
            _data = data;
            _gameStats = gameStats;
            foreach (var unit in _info.GetUnits())
            {
                _units.Add(new UnitModel(unit, _data.IsUnitOpened(unit.Tier)));
            }
        }

        public UnitModel GetUnitByTier(UnitTier tier)
        {
            return _units.Find(unit => unit.Tier == tier);
        }

        public void UpgradeFoodProduction()
        {
            if (Money < _gameStats.GetFoodProductionSpeedCost(FoodProductionLevel))
            {
                return;
            }
            
            Money -= (uint)_gameStats.GetFoodProductionSpeedCost(FoodProductionLevel);
            _data.FoodProductionLevel++;
            OnFoodProductionLevelChanged?.Invoke();
            OnMoneyChanged?.Invoke();
        }
        
        public void UpgradeTowerLevel()
        {
            if (Money < _gameStats.GetTowerUpgradeCost(TowerLevel))
            {
                return;
            }
            
            Money -= (uint)_gameStats.GetTowerUpgradeCost(TowerLevel);
            _data.TowerLevel++;
            OnTowerLevelChanged?.Invoke();
            OnMoneyChanged?.Invoke();
        }
        
        public void OpenUnit(UnitTier tier)
        {
            
            var unitModel = GetUnitByTier(tier);
            if (unitModel.IsUnitOpened || Money < unitModel.UnlockCost)
            {
                return;
            }
            
            Money -= (uint)unitModel.UnlockCost;
            
            _data.OpenUnit(tier);
            GetUnitByTier(tier).OpenUnit();
            OnUnitOpened?.Invoke();
            OnMoneyChanged?.Invoke();
        }

        
    }
}
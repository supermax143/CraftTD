using System;
using System.Collections.Generic;
using Core.Application.DataStorage.StorageItems;
using Core.Application.Info.Attributes.AttrimuteModdifiers;
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
        private TowerModel _tower;
        
        private readonly EpochInfo _info;
        private readonly EpochStorageData _data;
        private readonly GameStats _gameStats;

        
        
        public uint Money
        {
            get => _data.Money;
            set => _data.Money = value;
        }

        public string Name => _info.EpochName;
        public uint FoodProductionLevel => _data.FoodProductionLevel;
        public uint TowerLevel => _data.TowerLevel;
        public IEnumerable<UnitModel> Units => _units;
        public float FoodProductionSpeed => _gameStats.GetFoodProductionSpeed(FoodProductionLevel);
        public int FoodProductionUpgradeCost => _gameStats.GetFoodProductionSpeedCost(FoodProductionLevel);
        public int TowerUpgradeCost => _gameStats.GetTowerUpgradeCost(TowerLevel);
        public int TowerHealth => _gameStats.GetTowerHealth(TowerLevel);

        public TowerModel Tower => _tower;
        public EpochInfo Info => _info;

        internal EpochModel(EpochInfo info, EpochStorageData data, GameStats gameStats)
        {
            _info = info;
            _data = data;
            _gameStats = gameStats;
            
            AddTowers();
            
            foreach (var unit in _info.GetUnits())
            {
                _units.Add(new UnitModel(unit, _data.IsUnitOpened(unit.Tier)));
            }
        }

        private void AddTowers()
        {
            _tower = new TowerModel(_info.Tower);
            _tower.AddModifier(new HealthTowerAddModifier(GetHashCode(), TowerHealth));
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
            
            _tower.AddModifier(new HealthTowerAddModifier(GetHashCode(), TowerHealth));
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
using System;
using System.Collections.Generic;
using Core.Application.DataStorage.StorageItems;
using Core.Application.Info.Attributes.AttributeModifiers;
using Unity.Game;
using Unity.Game.Entity;

namespace Core.Application.Models
{
    public class EpochModel
    {
        public event Action<UnitModel> OnUnitOpened;
        public event Action OnFoodProductionLevelChanged;
        public event Action OnTowerLevelChanged;


        private readonly List<UnitModel> units = new();

        private TowerModel _tower;
        
        private readonly EpochInfo _info;
        private readonly EpochStorageData _data;
        private readonly GameStats _gameStats;
        private readonly int _epochNumber;
        private readonly Faction _faction;
        private readonly InventoryModel _inventory;

        public string Name => _info.EpochName;
        public uint FoodProductionLevel => _data.FoodProductionLevel;
        public uint TowerLevel => _data.TowerLevel;
        public float FoodProductionSpeed => _gameStats.GetFoodProductionSpeed(FoodProductionLevel);
        public int FoodProductionUpgradeCost => _gameStats.GetFoodProductionSpeedCost(FoodProductionLevel);
        public int TowerUpgradeCost => _gameStats.GetTowerUpgradeCost(TowerLevel);
        public int TowerHealth => _gameStats.GetTowerHealth(TowerLevel);
        public int EpochNumber => _epochNumber;

        
        public EpochInfo Info => _info;

        public TowerModel Tower => _tower;


        internal EpochModel(Faction faction, int epochNumber, EpochInfo info, EpochStorageData data, GameStats gameStats, InventoryModel inventory)
        {
            _faction = faction;
            _epochNumber = epochNumber;
            _info = info;
            _data = data;
            _gameStats = gameStats;
            _inventory = inventory;

            AddTower();
            AddUnits();
        }

        private void AddUnits()
        {
            foreach (var unitInfo in _info.GetUnits())
            {
                AddUnitModel(unitInfo.Tier, unitInfo);
            }
        }

        private void AddUnitModel(UnitTier tier, UnitInfo info)
        {
            if (TryGetUnitModel(tier, out _))
            {
                return;
            }
            
            var entity = GetUnitEntity(tier, _epochNumber);
            var modifiers = _info.GetUnitModifiers(tier);
            
            foreach (var modifierWrapper in modifiers)
            {
                var modifier = modifierWrapper.GetModifier();
                if (modifier != null)
                {
                    entity.AddModifier(modifier);
                }
            }
            var unitOpened = tier == UnitTier.Tier1 || _data.IsUnitOpened(tier);
            var unitModel = new UnitModel(info, entity, unitOpened);
            units.Add(unitModel);
        }
        
        
        private void AddTower()
        {
            _tower = new TowerModel(_info.Tower, GetTowerEntity(_epochNumber));
            if (_faction == Faction.Player)
            {
                _tower.AddModifier(new HealthAddModifier(GetHashCode(), TowerHealth));
            }
            /*else
            {
                _tower.AddModifier(new HealthOverrideModifier(GetHashCode(), 1));
            }*/
        }

        
        public bool TryGetUnitModel(UnitTier tier, out UnitModel unitModel)
        {
            unitModel = units.Find(unit => unit.Tier == tier);
            return unitModel != null;
        }

        private TowerEntity GetTowerEntity(int epochId)
        {
            return new TowerEntity(_faction, (uint)epochId, _gameStats);
        }
        
        private UnitEntity GetUnitEntity(UnitTier tier ,int epochId)
        {
            return new UnitEntity(tier, _faction, (uint)epochId, _gameStats);
        }
        
        public void UpgradeFoodProduction()
        {
            var cost = Resource.Money(_gameStats.GetFoodProductionSpeedCost(FoodProductionLevel));
            if (_inventory.Money < cost)
            {
                return;
            }

            _inventory.Money -= cost;
            _data.FoodProductionLevel++;
            OnFoodProductionLevelChanged?.Invoke();
        }
        
        public void UpgradeTowerLevel()
        {
            var cost = Resource.Money(_gameStats.GetTowerUpgradeCost(TowerLevel));
            if (_inventory.Money < cost)
            {
                return;
            }

            _inventory.Money -= cost;
            _data.TowerLevel++;

            Tower.AddModifier(new HealthAddModifier(GetHashCode(), TowerHealth));
            OnTowerLevelChanged?.Invoke();
        }
        
        public void OpenUnit(UnitTier tier)
        {
            if (!TryGetUnitModel(tier, out var unitModel) ||
                unitModel.IsUnitOpened ||
                _inventory.Money < Resource.Money(unitModel.UnlockCost))
            {
                return;
            }

            _inventory.Money -= Resource.Money(unitModel.UnlockCost);

            _data.OpenUnit(tier);
            unitModel.OpenUnit();
            OnUnitOpened?.Invoke(unitModel);
        }

        
    }
}

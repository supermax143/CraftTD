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
        public event Action OnUnitOpened;
        public event Action OnMoneyChanged;
        public event Action OnFoodProductionLevelChanged;
        public event Action OnTowerLevelChanged;
        
        private Dictionary<Faction, List<UnitModel>> _factionToUnits = new ();
        
        
        private List<TowerModel> _towers = new();
        
        
        
        private readonly EpochInfo _info;
        private readonly EpochStorageData _data;
        private readonly GameStats _gameStats;
        private readonly int _epochId;


        public uint Money
        {
            get => _data.Money;
            set => _data.Money = value;
        }

        public string Name => _info.EpochName;
        public uint FoodProductionLevel => _data.FoodProductionLevel;
        public uint TowerLevel => _data.TowerLevel;
        public float FoodProductionSpeed => _gameStats.GetFoodProductionSpeed(FoodProductionLevel);
        public int FoodProductionUpgradeCost => _gameStats.GetFoodProductionSpeedCost(FoodProductionLevel);
        public int TowerUpgradeCost => _gameStats.GetTowerUpgradeCost(TowerLevel);
        public int TowerHealth => _gameStats.GetTowerHealth(TowerLevel);

        
        public EpochInfo Info => _info;

        public TowerModel GetTower(Faction faction) => _towers.Find(tower => tower.Faction == faction);
        
        internal EpochModel(int currentEpochId, EpochInfo info, EpochStorageData data, GameStats gameStats)
        {
            _epochId = currentEpochId;
            _info = info;
            _data = data;
            _gameStats = gameStats;
            
            AddTowers();
            AddUnits();
        }

        private void AddUnits()
        {
            foreach (var unitInfo in _info.GetUnits())
            {
                AddUnitModel(Faction.Player, unitInfo.Tier, unitInfo);
                AddUnitModel(Faction.Enemy, unitInfo.Tier, unitInfo);
            }
        }

        private void AddUnitModel(Faction faction, UnitTier tier, UnitInfo info)
        {
            if (TryGetUnitModel(tier, faction, out _))
            {
                return;
            }
            
            if (!_factionToUnits.TryGetValue(faction, out List<UnitModel> units))
            {
                units = new List<UnitModel>();
                _factionToUnits.Add(faction, units);
            }
            
            var entity = GetUnitEntity(tier, faction, _epochId);
            var unitModel = new UnitModel(info, entity, tier == UnitTier.Tier1);
            units.Add(unitModel);
        }
        
        
        private void AddTowers()
        {
            var tower = new TowerModel(_info.Tower, GetTowerEntity(Faction.Player, _epochId));
            tower.AddModifier(new HealthAddModifier(GetHashCode(), TowerHealth));
            _towers.Add(tower);
            tower = new TowerModel(_info.Tower, GetTowerEntity(Faction.Enemy, _epochId));
            _towers.Add(tower);
        }

        
        public bool TryGetUnitModel(UnitTier tier, Faction faction, out UnitModel unitModel)
        {
            unitModel = null;
            if (!_factionToUnits.TryGetValue(faction, out List<UnitModel> units))
            {
                return false;
            }
            
            unitModel = units.Find(unit => unit.Tier == tier);
            return unitModel != null;
        }

        private TowerEntity GetTowerEntity(Faction faction, int epochId)
        {
            return new TowerEntity(faction, (uint)epochId, _gameStats);
        }
        
        private UnitEntity GetUnitEntity(UnitTier tier ,Faction faction, int epochId)
        {
            return new UnitEntity(tier, faction,(uint)epochId, _gameStats);
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
            
            GetTower(Faction.Player).AddModifier(new HealthAddModifier(GetHashCode(), TowerHealth));
            OnTowerLevelChanged?.Invoke();
            OnMoneyChanged?.Invoke();
        }
        
        public void OpenUnit(UnitTier tier)
        {
            if (!TryGetUnitModel(tier, Faction.Player, out var unitModel) || 
                unitModel.IsUnitOpened || 
                Money < unitModel.UnlockCost)
            {
                return;
            }
            
            Money -= (uint)unitModel.UnlockCost;
            
            _data.OpenUnit(tier);
            unitModel.OpenUnit();
            OnUnitOpened?.Invoke();
            OnMoneyChanged?.Invoke();
        }

        
    }
}
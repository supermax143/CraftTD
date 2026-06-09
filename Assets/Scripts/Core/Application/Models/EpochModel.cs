using System;
using System.Collections.Generic;
using Core.Application.DataStorage.StorageItems;
using Core.Application.Interfaces.Info;
using Unity.Game;

namespace Core.Application.Models
{
    public class EpochModel
    {
        public event Action OnUnitOpened;
        public event Action OnMoneyChanged;
        
        private readonly List<UnitModel> _units = new();
        private readonly IEpochInfo _info;
        private readonly EpochStorageData _data;

        public uint Money
        {
            get => _data.Money;
            set => _data.Money = value;
        }

        public uint FoodProductionLevel => _data.FoodProductionLevel;
        public uint TowerUpgradeLevel => _data.TowerUpgradeLevel;
        public IEnumerable<UnitModel> Units => _units;
        
        internal EpochModel(IEpochInfo info, EpochStorageData data)
        {
            _info = info;
            _data = data;
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
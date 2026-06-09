using System.Collections.Generic;
using Core.Application.DataStorage;
using Zenject;

namespace Unity.Game
{
    public class EpochManager
    {
        
        [Inject] private IDataStorage _dataStorage; 
        [Inject] private ChronologyInfo _chronologyInfo;
        
        public bool TryGetCurrentEpoch(out EpochInfo epoch)
        {
            return _chronologyInfo.TryGetEpoch(_dataStorage.CurrentEpochIndex, out epoch);
        }
        
        public void SetEpoch(int index)
        {
            if (_chronologyInfo.Epochs.Count >= index || index < 0)
            {
                return;
            }
            
            _dataStorage.SetCurrentEpoch(index);
        }
        
        public IEnumerable<EpochInfo> GetEpochs() => _chronologyInfo.Epochs;
        
        public bool TryGetUnitDataByTier(UnitTier tier, out UnitEntityInfo unit)
        {
            if (TryGetCurrentEpoch(out var epoch))
            {
                unit = epoch.GetUnitDataByTier(tier);
                return true;
            }
            unit = default;
            return false;
        }
    }
}
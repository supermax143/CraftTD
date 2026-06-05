using Core.Application.DataStorage;
using Zenject;

namespace Unity.Game
{
    public class EpochManager
    {
        
        [Inject] private IDataStorage _dataStorage; 
        [Inject] private ChronologyData _chronologyData;
        
        public bool TryGetCurrentEpoch(out EpochData epoch)
        {
            return _chronologyData.TryGetEpoch(_dataStorage.CurrentEpochIndex, out epoch);
        }
        
        /*public bool TryGetNextEpoch(out EpochData epoch)
        {
            return _chronologyData.TryGetEpoch(_dataStorage.CurrentEpochIndex + 1, out epoch);
        }*/
        
        
        
        public bool TryGetUnitDataByTier(UnitTier tier, out UnitEntityData unit)
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
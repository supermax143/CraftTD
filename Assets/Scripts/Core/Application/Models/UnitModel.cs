using Unity.Game;

namespace Core.Application.Models
{
    public class UnitModel
    {
        private readonly UnitEntityInfo _info;
        private bool _isUnitOpened;

        public bool IsUnitOpened => _isUnitOpened;
        public UnitTier Tier => _info.Tier;
        public int FoodCost => _info.FoodCost;
        public int UnlockCost => _info.UnlockCost;
        
        public void OpenUnit()
        {
            _isUnitOpened = true;
        }
        
        public UnitModel(UnitEntityInfo info, bool isUnitOpened)
        {
            _info = info;
            _isUnitOpened = isUnitOpened;
        }

    }
}
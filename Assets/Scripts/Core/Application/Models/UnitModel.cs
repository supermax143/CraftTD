using Unity.Game;

namespace Core.Application.Models
{
    public class UnitModel
    {
        private readonly UnitEntityInfo _info;
        private bool _isUnitOpened;

        public bool IsUnitOpened => _isUnitOpened;
        public UnitTier Tier => Info.Tier;
        public int FoodCost => Info.FoodCost;
        public int UnlockCost => Info.UnlockCost;

        public UnitEntityInfo Info => _info;

        
        
        public void OpenUnit()
        {
            _isUnitOpened = true;
        }
        
        public UnitModel(UnitEntityInfo info, bool isUnitOpened, GameStats stats, int epochId)
        {
            _info = info;
            _info.Initialize(stats, (uint)epochId);
            _isUnitOpened = isUnitOpened;
            
        }

    }
}
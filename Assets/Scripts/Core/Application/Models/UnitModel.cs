using Core.Application.Interfaces.Info;
using Unity.Game;

namespace Core.Application.Models
{
    public class UnitModel
    {
        private readonly IUnitInfo _info;
        private readonly bool _isUnitOpened;

        public bool IsUnitOpened => _isUnitOpened;
        public UnitTier Tier => _info.Tier;
        public int FoodCost => _info.FoodCost;
        public int UnlockCost => _info.UnlockCost;
        
        public UnitModel(IUnitInfo info, bool isUnitOpened)
        {
            _info = info;
            _isUnitOpened = isUnitOpened;
        }

    }
}
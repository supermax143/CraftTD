using Unity.Game;
using Unity.Game.Entity;

namespace Core.Application.Models
{
    public class UnitModel
    {

        private readonly UnitInfo _info;
        private bool _isUnitOpened;
        private readonly UnitEntity _entity;
        private readonly Faction _faction;
        private readonly int _epochId;


        public bool IsUnitOpened => _isUnitOpened;
        public UnitTier Tier => Info.Tier;
        public int FoodCost => _entity.FoodCost;
        public int UnlockCost => _entity.UnlockCost;
        public UnitInfo Info => _info;

        public UnitEntity Entity => _entity;

        public Faction Faction => _faction;

        public int EpochId => _epochId;


        public void OpenUnit()
        {
            _isUnitOpened = true;
        }
        
        public UnitModel(UnitInfo info, UnitEntity entity, Faction faction, int epochId, bool isUnitOpened)
        {
            _info = info;
            _entity = entity;
            _isUnitOpened = isUnitOpened;
            _faction = faction;
            _epochId = epochId;
        }

    }
}
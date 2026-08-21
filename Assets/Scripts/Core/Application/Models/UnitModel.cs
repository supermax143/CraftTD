using Unity.Game;
using Unity.Game.Entity;

namespace Core.Application.Models
{
    public class UnitModel
    {

        private readonly UnitInfo _info;
        private bool _isUnitOpened;
        private bool _isDefenseStanceOpened;
        private readonly UnitEntity _entity;


        public bool IsUnitOpened => _isUnitOpened;
        public bool IsDefenseStanceOpened => _isDefenseStanceOpened;
        public UnitTier Tier => Info.Tier;
        public int FoodCost => _entity.FoodCost;
        public int UnlockCost => _entity.UnlockCost;
        public int DefenseStanceCost => _entity.DefenseStanceCost;
        public UnitInfo Info => _info;

        public UnitEntity Entity => _entity;


        public void OpenUnit()
        {
            _isUnitOpened = true;
        }

        public void OpenDefenseStance()
        {
            _isDefenseStanceOpened = true;
        }

        public UnitModel(UnitInfo info, UnitEntity entity, bool isUnitOpened, bool isDefenseStanceOpened = false)
        {
            _info = info;
            _entity = entity;
            _isUnitOpened = isUnitOpened;
            _isDefenseStanceOpened = isDefenseStanceOpened;
        }

    }
}
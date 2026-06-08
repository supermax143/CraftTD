using Core.Application.Interfaces.Info;

namespace Core.Application.Models
{
    public class UnitModel
    {
        private readonly IUnitInfo _info;

        public UnitModel(IUnitInfo info)
        {
            _info = info;
        }
    }
}
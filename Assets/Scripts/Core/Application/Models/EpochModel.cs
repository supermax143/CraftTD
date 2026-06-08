using System.Collections.Generic;
using Core.Application.Interfaces.Info;

namespace Core.Application.Models
{
    public class EpochModel
    {
        
        private readonly List<UnitModel> _units = new();
        private readonly IEpochInfo _info;
        
        internal EpochModel(IEpochInfo info, DataStorage.DataStorage dataStorage)
        {
            _info = info;
            foreach (var unit in _info.GetUnits())
            {
                _units.Add(new UnitModel(unit));
            }
        }
    }
}
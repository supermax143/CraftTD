using System.Collections.Generic;

namespace Core.Application.Interfaces.Info
{
    public interface IEpochInfo
    {
        IEnumerable<IUnitInfo> GetUnits();
    }
}
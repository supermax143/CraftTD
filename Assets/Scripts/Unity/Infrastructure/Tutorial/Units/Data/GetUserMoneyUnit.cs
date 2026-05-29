using Core.Application.DataStorage;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;
using Zenject;

namespace Unity.Infrastructure.Tutorial.Units.Data
{
    [UnitCategory("Custom/Data")]
    [UnitTitle("GetUserMoney")]
    public class GetUserMoneyUnit : CustomGetUnit<uint>
    {
        private const string RESULT = "result";
        
        [Inject] private IDataStorage _dataStorage;
        
        protected override uint GetResult(Flow flow)
        {
            return _dataStorage.UserMoney;
        }
    }
}

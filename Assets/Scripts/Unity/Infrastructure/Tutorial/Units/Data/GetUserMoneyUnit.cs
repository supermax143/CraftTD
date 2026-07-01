using Core.Application.DataStorage;
using Core.Application.Models;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;
using Zenject;

namespace Unity.Infrastructure.Tutorial.Units.Data
{
    [UnitCategory("Custom/Data")]
    [UnitTitle("GetUserMoney")]
    public class GetUserMoneyUnit : CustomGetUnit<int>
    {
        private const string RESULT = "result";

        [Inject] private IMainModel _model;

        protected override int GetResult(Flow flow)
        {
            return _model.Inventory.Money.Value;
        }
    }
}

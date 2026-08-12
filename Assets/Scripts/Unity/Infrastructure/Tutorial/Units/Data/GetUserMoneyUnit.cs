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

        [Inject] private IInventoryModel _inventory;

        protected override int GetResult(Flow flow)
        {
            return _inventory.GetResourceCount(ResourceType.Money);
        }
    }
}

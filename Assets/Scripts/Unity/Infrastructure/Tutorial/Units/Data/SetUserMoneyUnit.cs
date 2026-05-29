using System.Collections.Generic;
using Core.Application.DataStorage;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;
using Zenject;

namespace Unity.Infrastructure.Tutorial.Units.Data
{
    [UnitCategory("Custom/Data")]
    [UnitTitle("SetUserMoney")]
    public class SetUserMoneyUnit : CustomUnit
    {
        private const string MONEY = "money";
        
        [Inject] private IDataStorage _dataStorage;
        
        protected override IEnumerable<IUnitValuePort> DefineValuePortsInternal()
        {
            yield return ValueInput(MONEY, 0u);
        }
        
        protected override void OnExecute(Flow flow)
        {
            var money = GetValue<uint>(flow, MONEY);
            _dataStorage.SetMoney(money);
        }
    }
}

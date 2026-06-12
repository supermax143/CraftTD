using System.Collections.Generic;
using Core.Application.DataStorage;
using Core.Application.Models;
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
        
        [Inject] private IMainModel _model;
        
        protected override IEnumerable<IUnitValuePort> DefineValuePortsInternal()
        {
            yield return ValueInput(MONEY, 0);
        }

        protected override void OnExecute(Flow flow)
        {
            var money = GetValue<int>(flow, MONEY);
            _model.Money = Resource.Money(money);
        }
    }
}

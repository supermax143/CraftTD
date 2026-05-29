using System.Collections.Generic;
using System.Linq;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;

namespace Unity.Infrastructure.Tutorial.Units
{
    [UnitCategory("Custom")]
    [UnitTitle("GetGUIElement")]
    internal class GetGUIElementUnit : CustomUnit
    {
        private const string IDENT = "ident";
        private const string RESULT = "result";

        protected override IEnumerable<IUnitValuePort> DefineValuePortsInternal()
        {
            yield return ValueInput(IDENT, string.Empty);
            yield return ValueOutput<TutorialTag>(RESULT);
        }

        protected override void OnExecute(Flow flow)
        {
            var ident = GetValue<string>(flow, IDENT);

            if (string.IsNullOrWhiteSpace(ident)) return;
            var items = TutorialTag.GetByIdent(ident);
            SetValue(flow, RESULT, items.FirstOrDefault());
        }
    }
}
using System.Collections.Generic;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;

namespace Unity.Infrastructure.Tutorial.Units.Time
{
    [UnitCategory("Custom/Time")]
    [UnitTitle("PauseTime")]
    public class PauseTimeUnit : CustomUnit
    {
        protected override IEnumerable<IUnitValuePort> DefineValuePortsInternal()
        {
            yield break;
        }

        protected override void OnExecute(Flow flow)
        {
            UnityEngine.Time.timeScale = 0f;
        }
    }
}

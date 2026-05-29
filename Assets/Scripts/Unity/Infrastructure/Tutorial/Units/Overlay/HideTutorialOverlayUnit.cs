using System.Collections.Generic;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;
using Zenject;

namespace Unity.Infrastructure.Tutorial.Units.Overlay
{
    [UnitCategory("Custom/Popup")]
    [UnitTitle("HideTutorialOverlay")]
    public class HideTutorialOverlayUnit : CustomUnit
    {
        [Inject] private ITutorialOverlayController _tutorialOverlay;
        
        protected override IEnumerable<IUnitValuePort> DefineValuePortsInternal()
        {
            yield break;
        }
        
        protected override void OnExecute(Flow flow)
        {
            _tutorialOverlay.HideOverlay();
        }
    }
}

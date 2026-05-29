using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;

namespace Unity.Infrastructure.Tutorial.Units
{
    [UsedImplicitly]
    [UnitCategory("Custom")]
    [UnitTitle("SendCompleteTutorial")]
    public class SendCompleteTutorialUnit : CustomUnit
    {
        /*[DoNotSerialize] [PortLabelHidden] private ControlInput _inputTrigger;

        protected override void Definition()
        {
            _inputTrigger = ControlInput(nameof(_inputTrigger), func);

            ControlOutput func(Flow flow)
            {
                EventBus.Trigger(TutorialTask.TASK_COMPLETE_EVENT, flow.stack.rootGraph.title);
                return null;
            }
        }*/
        protected override IEnumerable<IUnitValuePort> DefineValuePortsInternal()
        {
            yield break;
        }

        protected override void OnExecute(Flow flow)
        {
            EventBus.Trigger(TutorialTask.TASK_COMPLETE_EVENT, flow.stack.rootGraph.title);
        }
    }
}
using System;
using Unity.Infrastructure.VisualActions.ActionsData;

namespace Unity.Infrastructure.VisualActions
{
    public class ActionsDispatcher : IActionsDispatcher
    {
        public event Action<IActionData, bool> OnActionAdded;

        public void AddAction(IActionData data, bool instant = false)
        {
            OnActionAdded?.Invoke(data, instant);
        }
    }
}
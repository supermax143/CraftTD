using System;
using Unity.Infrastructure.VisualActions.ActionsData;

namespace Unity.Infrastructure.VisualActions
{
    public class ActionsDispatcher : IActionsDispatcher
    {
        public event Action<IActionData> OnActionAdded;

        public void AddAction(IActionData data)
        {
            OnActionAdded?.Invoke(data);
        }
    }
}
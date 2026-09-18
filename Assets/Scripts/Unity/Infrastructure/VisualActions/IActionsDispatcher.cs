using System;
using Unity.Infrastructure.VisualActions.ActionsData;

namespace Unity.Infrastructure.VisualActions
{
    public interface IActionsDispatcher
    {
        event Action<IActionData, bool> OnActionAdded;
        void AddAction(IActionData data, bool instant = false);
    }
}
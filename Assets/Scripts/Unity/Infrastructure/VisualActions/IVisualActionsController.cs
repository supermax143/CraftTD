using Unity.Infrastructure.VisualActions.ActionsData;

namespace Unity.Infrastructure.VisualActions
{
    public interface IVisualActionsController
    {
        void AddAction(IActionData actionData);
    }
}
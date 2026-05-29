using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Exploration.Controllers.Map.GameLevelPresenter
{
    public static class TaskExceptionHandler
    {
        [RuntimeInitializeOnLoadMethod]
#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#endif
        private static void ProcessUnobservedTask()
        {
            TaskScheduler.UnobservedTaskException += (obj, exArg) =>
            {
                exArg.SetObserved();
                Debug.LogError($"Handled unobserved exception, {exArg.Exception.GetType()} with message: {exArg.Exception.Message}", obj as Object);
                Debug.LogException(exArg.Exception);
            };
        }
    }
}
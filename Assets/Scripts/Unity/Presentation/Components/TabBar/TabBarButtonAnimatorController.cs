using UnityEngine;

namespace Unity.Presentation.Components.TabBar
{
    /// <summary>
    /// Контроллер анимации для TabBarButton
    /// </summary>
    public class TabBarButtonAnimatorController : AnimatorControllerBase
    {
        private static class Flags
        {
            public static readonly int IsSelected = Animator.StringToHash(nameof(IsSelected));
            public static readonly int IsDisabled = Animator.StringToHash(nameof(IsDisabled));
        }

        public void SetSelectedState(bool value)
        {
            SetBool(Flags.IsSelected, value);
        }

        public void SetDisabledState(bool value)
        {
            SetBool(Flags.IsDisabled, value);
        }
    }
}

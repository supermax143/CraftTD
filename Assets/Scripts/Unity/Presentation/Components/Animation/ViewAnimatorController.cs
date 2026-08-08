using UnityEngine;

namespace Unity.Presentation.Components
{
    public class ViewAnimatorController : AnimatorControllerBase
    {
        private static class States
        {
            public static readonly int Visible = Animator.StringToHash(nameof(Visible));
        }

        public void Show()
        {
            SetBool(States.Visible, true);
        }

        public void Hide()
        {
            SetBool(States.Visible, false);
        }
    }
}

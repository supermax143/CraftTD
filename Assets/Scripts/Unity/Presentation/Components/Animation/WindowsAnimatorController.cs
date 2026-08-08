using UnityEngine;

namespace Unity.Presentation.Components
{
    public class WindowsAnimatorController : AnimatorControllerBase
    {
        private static class Triggers
        {
            public static readonly int Show = Animator.StringToHash(nameof(Show));
            public static readonly int Hide = Animator.StringToHash(nameof(Hide));
        }

        public void Show()
        {
            PlayAnimation(Triggers.Show);
        }

        public void Hide()
        {
            PlayAnimation(Triggers.Hide);
        }
    }
}

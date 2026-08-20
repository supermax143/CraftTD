using Unity.Presentation.Components;
using UnityEngine;

namespace Unity.Infrastructure.Effects.AnimatedPopup
{
    public class PopupAnimatorController : AnimatorControllerBase
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
using UnityEngine;

namespace Unity.Presentation.Components
{
    public class HUDGameAnimatorController : AnimatorControllerBase
    {
        private static class Flags
        {
            public static readonly int IsUpgradeState = Animator.StringToHash(nameof(IsUpgradeState));
        }
        
        public void SetIsUpgradeState(bool value)
        {
            SetBool(Flags.IsUpgradeState, value);
        }
        
    }
}
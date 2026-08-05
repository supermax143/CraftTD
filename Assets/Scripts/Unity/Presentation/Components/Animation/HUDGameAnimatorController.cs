using UnityEngine;

namespace Unity.Presentation.Components
{
    public class HUDGameAnimatorController : AnimatorControllerBase
    {
        private static class Flags
        {
            public static readonly int IsBattle = Animator.StringToHash(nameof(IsBattle));
        }
        
        public void SetIsBattleState(bool value)
        {
            SetBool(Flags.IsBattle, value);
        }
        
    }
}
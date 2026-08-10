using UnityEngine;

namespace Unity.Presentation.Components
{
    public class BattleViewAnimatorController : ViewAnimatorController
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
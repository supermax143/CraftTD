using System;
using UnityEngine;

namespace Unity.Presentation.Components
{
    public class UnitAnimationEvents : MonoBehaviour
    {
        
        public event Action OnAttackActivate;
        public event Action OnAttackAnimationStart;
        
        public void AttackAnimationStart()
        {
            OnAttackAnimationStart?.Invoke();
        }
        
        public void AttackActivate()
        {
            OnAttackActivate?.Invoke();
        }
        
    }
}
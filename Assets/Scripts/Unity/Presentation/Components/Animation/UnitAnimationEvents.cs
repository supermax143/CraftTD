using System;
using UnityEngine;

namespace Unity.Presentation.Components
{
    public class UnitAnimationEvents : MonoBehaviour
    {
        
        public event Action OnAttackActivate;
        
        
        public void AttackActivate()
        {
            OnAttackActivate?.Invoke();
        }
    }
}
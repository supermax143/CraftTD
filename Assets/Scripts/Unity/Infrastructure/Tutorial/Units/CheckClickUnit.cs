using System.Reflection;
using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;
using Pointer = UnityEngine.InputSystem.Pointer;

namespace Unity.Infrastructure.Tutorial.Units
{
    [UnitCategory("Custom/Input")]
    [UnitTitle("CheckClick")]
    public class CheckClickUnit : CustomGetUnit<bool>
    {
        
        private bool _wasPressed = false;

        protected override bool GetResult(Flow flow)
        {
            
            bool isCurrentlyPressed = false;
            

            // Для мышки - проверяем клик (нажатие и отпускание)
            if (Pointer.current.press.wasPressedThisFrame)
            {
                _wasPressed = true;
            }
            
            if (_wasPressed && Pointer.current.press.wasReleasedThisFrame)
            {
                _wasPressed = false;
                return true;
            }

            
            return false;
        }
    }
}

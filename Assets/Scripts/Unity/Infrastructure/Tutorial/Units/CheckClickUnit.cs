using Unity.Infrastructure.Tutorial.Units.BaseUnits;
using Unity.VisualScripting;
using UnityEngine;

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
            
#if UNITY_EDITOR || UNITY_WEBGL || UNITY_STANDALONE
            // Для мышки - проверяем клик (нажатие и отпускание)
            if (Input.GetMouseButtonDown(0))
            {
                _wasPressed = true;
            }
            
            if (_wasPressed && Input.GetMouseButtonUp(0))
            {
                _wasPressed = false;
                return true;
            }
#else
            // для мобильных устройств - проверяем тач
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    _wasPressed = true;
                }
                else if (_wasPressed && touch.phase == TouchPhase.Ended)
                {
                    _wasPressed = false;
                    return true;
                }
            }
#endif
            
            return false;
        }
    }
}

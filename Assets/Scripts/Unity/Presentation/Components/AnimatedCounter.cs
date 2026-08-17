using System.Collections;
using Shared.Utils;
using TMPro;
using Unity.Utils.Time;
using UnityEngine;
using UnityEngine.UI;

namespace Zombies
{
    public class AnimatedCounter : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _textField;
        [SerializeField]
        private float _animationDuration = 0.5f;
        
        private int _currentValue;
        private bool _initialized = false;
        private Timer _timer = new();
        private Coroutine _coroutine;
        
        public void SetValue(int value)
        {
            if (_currentValue == value && _initialized)
            {
                return;
            }
            
            if (!_initialized)
            {
                SetText(value);
            }
            else
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                    _coroutine = null;
                }
                _coroutine = StartCoroutine(Animate(value));
            }
            _currentValue = value;
            _initialized = true;
        }

        private IEnumerator Animate(int value)
        {
            float startValue = _currentValue;
            float endValue = value;
            
            _timer.Start(_animationDuration);
            while (!_timer.IsComplete)
            {
                var progress = _timer.Progress;
                SetText((int)Mathf.Round(Mathf.Lerp(startValue, endValue, progress)));
               yield return  null;
            }
            SetText(value);
            _coroutine = null;
        }
        
        private void SetText(int value)
        {
            _textField.text = LargeNumberFormatter.Format(value);
        }
    }
}
using System;
using System.Collections;
using Unity.Mathematics;
using Unity.Utils.Time;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Presentation.Components
{
    [RequireComponent(typeof(Slider))]
    public class AnimatedProgressbar : MonoBehaviour
    {
        [SerializeField]
        private Slider _progressSlider;
        
        private float _animationTime = 0.5f;
        
        private Coroutine _coroutine;
        private readonly Timer _timer = new Timer();

        private void OnValidate()
        {
            _progressSlider ??= GetComponent<Slider>();
        }

        public void SetProgress(float progress)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            _coroutine = StartCoroutine(AnimateProgress(Mathf.Clamp01(progress)));
        }
        
        private IEnumerator AnimateProgress(float targetValue)
        {
            var startValue = _progressSlider.value;
            _timer.Start(_animationTime);
            
            while (!_timer.IsComplete)
            {
                _progressSlider.value = Mathf.Lerp(startValue, targetValue, _timer.Progress);
                yield return null;
            }
            _progressSlider.value = targetValue;
            _coroutine = null;
        }
        
    }
}
using System.Collections;
using Unity.Utils.Time;
using UnityEngine;

namespace Utils.ColorEffects
{
    public class BlinkEffect : TintController
    {
        [SerializeField]
        private float _blinkDuration = 0.5f;

        private Coroutine _blinkCoroutine;
        
        private Timer _blinkTimer = new Timer();
        
        public void Show()
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
            }
            _blinkCoroutine = StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            _blinkTimer.Start(_blinkDuration/2);
            var startValue = 0f;
            var targetValue = 1f;
            while (!_blinkTimer.IsComplete)
            {
                SetTintAlpha(Mathf.Lerp(startValue, targetValue, _blinkTimer.Progress));
                yield return null;
            }
            
            SetTintAlpha(targetValue);
            _blinkTimer.Start(_blinkDuration/2);
            startValue = 1f;
            targetValue = 0f;
            while (!_blinkTimer.IsComplete)
            {
                SetTintAlpha(Mathf.Lerp(startValue, targetValue, _blinkTimer.Progress));
                yield return null;
            }
            
            SetTintAlpha(targetValue);
            _blinkCoroutine = null;
        }
        
        private void SetTintAlpha(float alpha)
        {
            var color = _tintColor;
            color.a = alpha;
            SetTintColor(color);
        }
        
    }
}
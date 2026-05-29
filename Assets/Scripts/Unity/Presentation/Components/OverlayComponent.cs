using System.Collections;
using Unity.Utils.Time;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Presentation.Components
{
    public class OverlayComponent : MonoBehaviour
    {
        
        [SerializeField]
        private float _maxAlpha = 0.7f;
        [SerializeField]
        private float _animationDuration = 0.5f;
        [SerializeField, HideInInspector]
        private Image _image;
        
        private readonly Timer _timer = new();
        private Coroutine _coroutine;

        private void OnValidate()
        {
            _image = GetComponent<Image>();
        }

        public void Show(float targetAlpha = -1)
        {
            var alpha = targetAlpha >= 0 ? targetAlpha : _maxAlpha; 
            gameObject.SetActive(true);
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(Animate(alpha));
        }
        public void Hide()
        {
            gameObject.SetActive(true);
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(Animate(0));
        }

        private IEnumerator Animate(float targetAlpha)
        {
            var startAlpha = _image.color.a;
            _timer.Start(_animationDuration);
            while (!_timer.IsComplete)
            {
                SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, _timer.Progress));
                yield return null;
            }
            SetAlpha(targetAlpha);
            if (targetAlpha == 0)
            {
                gameObject.SetActive(false);
            }
        }

        private void SetAlpha(float alpha)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
        }
    }
}
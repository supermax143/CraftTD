using System.Collections;
using UnityEngine;

namespace Unity.Infrastructure.Camera
{
    /// <summary>
    /// Компонент для тряски камеры с настраиваемой амплитудой и длительностью
    /// </summary>
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField]
        private float _shakeDuration = .5f;
        
        [SerializeField]
        private Vector2 _maxShakeDelta = new(0.5f, 0.5f);
        
        private UnityEngine.Camera _camera;
        private Coroutine _shakeCoroutine;
        private Vector3 _originalPosition;

        public void Initialize(UnityEngine.Camera camera)
        {
            _camera = camera;
        }
        
        public void Shake(float duration = 0)
        {
            if (duration == 0)
            {
                duration = _shakeDuration;
            }
            
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }
            else
            {
                _originalPosition = _camera.transform.localPosition;
            }
            
            _shakeCoroutine = StartCoroutine(AnimateShake(duration));
        }

        private IEnumerator AnimateShake(float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;
                float damping = 1f - progress;

                float offsetX = Random.Range(-_maxShakeDelta.x, _maxShakeDelta.x) * damping;
                float offsetY = Random.Range(-_maxShakeDelta.y, _maxShakeDelta.y) * damping;

                _camera.transform.localPosition = _originalPosition + new Vector3(offsetX, offsetY, 0f);

                yield return null;
            }

            _camera.transform.localPosition = _originalPosition;
            _shakeCoroutine = null;
        }
    }
}
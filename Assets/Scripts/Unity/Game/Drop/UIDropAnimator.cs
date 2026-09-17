using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity.Game
{
    /// <summary>
    /// Аниматор для UI дропов - разлет ресурсов радиально с задержкой
    /// </summary>
    public class UIDropAnimator : MonoBehaviour
    {
        [SerializeField]
        private float _minDistance = 50f;
        [SerializeField]
        private float _maxDistance = 100f;
        [SerializeField]
        private float _scatterDuration = 0.3f;
        [SerializeField]
        private float _minDelay = 0f;
        [SerializeField]
        private float _maxDelay = 0.1f;

        public void Show(RectTransform target, Action<RectTransform> onComplete = null)
        {
            float angle = Random.Range(0f, 360f);
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            float distance = Random.Range(_minDistance, _maxDistance);
            Vector2 offset = direction * distance;
            float delay = Random.Range(_minDelay, _maxDelay);

            Sequence seq = DOTween.Sequence();

            seq.AppendInterval(delay);
            seq.Append(target.DOAnchorPos(target.anchoredPosition + offset, _scatterDuration)
                .SetEase(Ease.OutQuad));
            seq.onComplete += () =>
            {
                onComplete?.Invoke(target);
            };
        }
    }
}

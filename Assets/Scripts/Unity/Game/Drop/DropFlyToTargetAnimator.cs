using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unity.Game
{
    public class DropFlyToTargetAnimator : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        [Header("Ссылки")]
        [SerializeField] private Camera _mainCam;

        [Header("Параметры полета")]
        [SerializeField] private float _flightDuration = 0.6f;
        [SerializeField] private float _arcHeight = 2.5f;   // Высота дуги полета
        

        private Vector3 _initialScale;
        private Dictionary<RectTransform, float> _cachedIconHeights = new Dictionary<RectTransform, float>();

        private void Awake()
        {
            if (_mainCam == null) _mainCam = Camera.main;
        }

        private void CacheIconWorldHeight(RectTransform icon)
        {
            if (_cachedIconHeights.ContainsKey(icon))
            {
                return;
            }

            float iconWorldHeight;
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                Vector3[] corners = new Vector3[4];
                icon.GetWorldCorners(corners);
                iconWorldHeight = corners[1].y - corners[0].y;
            }
            else if (_mainCam.orthographic)
            {
                float pixelToWorld = (_mainCam.orthographicSize * 2f) / Screen.height;
                float iconPixelHeight = icon.rect.height * icon.lossyScale.y;
                iconWorldHeight = iconPixelHeight * pixelToWorld;
            }
            else
            {
                iconWorldHeight = 1f;
            }

            _cachedIconHeights[icon] = iconWorldHeight;
        }

        /// <summary>
        /// Запускает полет дропа в UI иконку
        /// </summary>
        public void FlyToIcon(RectTransform targetIcon,Transform flyingObject, float delay, Action<Transform> onComplete)
        {
            CacheIconWorldHeight(targetIcon);
            _initialScale = flyingObject.localScale;
            
            // 1. Получаем целевую позицию в мировых координатах
            Vector3 targetWorldPos;
            if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                // Для Screen Space - Camera используем мировые координаты напрямую
                Vector3[] corners = new Vector3[4];
                targetIcon.GetWorldCorners(corners);
                targetWorldPos = (corners[0] + corners[2]) / 2f;
            }
            else
            {
                // Для Overlay используем экранные координаты
                Vector3 iconScreenPos = GetUIScreenPosition(targetIcon);
                float zDistance = Mathf.Abs(_mainCam.transform.position.z - transform.position.z);
                targetWorldPos = _mainCam.ScreenToWorldPoint(new Vector3(iconScreenPos.x, iconScreenPos.y, zDistance));
            }

            // 3. Рассчитываем целевой масштаб (чтобы дроп стал ровно размером с иконку)
            float dropSize = GetDropSize(flyingObject);
            Vector3 targetScale = CalculateTargetScale(targetIcon, dropSize);

            // 4. Настраиваем точки для кривой Безье (красивая дуга)
            Vector3 startPos = flyingObject.position;
            Vector3 midPoint = (startPos + targetWorldPos) / 2f + Vector3.up * _arcHeight;

            // Убиваем старые твины
            DOTween.Kill(flyingObject);
            
            // Сбрасываем состояние перед полетом
            flyingObject.localScale = _initialScale;
            flyingObject.rotation = Quaternion.identity;

            // Настраиваем сортировку выше UI
            /*var spriteRenderer = flyingObject.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Windows";
                spriteRenderer.sortingOrder = 100;
            }*/
            

            var seq = DOTween.Sequence();

            
            
            seq.Append(
                DOTween.To(
                        () => 0f,
                        t => UpdateFlight(flyingObject, t, startPos, midPoint, targetWorldPos, targetScale),
                        1f,
                        _flightDuration
                    )
                    .SetEase(Ease.InCubic) // InBack дает эффект "всасывания" в UI в конце
                    .OnComplete(() =>
                    {
                        onComplete.Invoke(flyingObject);
                    })).SetDelay(delay);

        }

        /// <summary>
        /// Вычисляется каждый кадр DOTween'ом. Двигает, масштабирует и поворачивает объект.
        /// </summary>
        private void UpdateFlight(Transform flyingObject, float t, Vector3 p0, Vector3 p1, Vector3 p2,
            Vector3 targetScale)
        {
            // --- КВАДРАТИЧНАЯ КРИВАЯ БЕЗЬЕ ---
            // Формула: B(t) = (1-t)^2 * P0 + 2 * (1-t) * t * P1 + t^2 * P2
            float oneMinusT = 1f - t;
            Vector3 pos = oneMinusT * oneMinusT * p0 
                        + 2f * oneMinusT * t * p1 
                        + t * t * p2;
            
            flyingObject.position = pos;

            // --- МАСШТАБ ---
            // Плавно интерполируем от начального к целевому
            flyingObject.localScale = Vector3.Lerp(_initialScale, targetScale, t);

            // --- ПОВОРОТ (Look at direction) ---
            // Чтобы дроп летел "носиком" вперед по траектории
            /*if (t < 0.99f)
            {
                // Берем следующую точку на кривой для расчета вектора направления
                float nextT = Mathf.Min(t + 0.01f, 1f);
                float oneMinusNextT = 1f - nextT;
                Vector3 nextPos = oneMinusNextT * oneMinusNextT * p0 
                                + 2f * oneMinusNextT * nextT * p1 
                                + nextT * nextT * p2;
                
                Vector3 dir = (nextPos - pos).normalized;
                if (dir != Vector3.zero)
                {
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    flyingObject.rotation = Quaternion.Euler(0, 0, angle);
                }
            }*/
        }

        /// <summary>
        /// Рассчитывает, во сколько раз нужно увеличить/уменьшить дроп, 
        /// чтобы он визуально совпал с UI иконкой.
        /// </summary>
        private float GetDropSize(Transform obj)
        {
            var spriteRenderer = obj.GetComponentInChildren<SpriteRenderer>();
            return spriteRenderer.bounds.size.y;
        }

        private Vector3 CalculateTargetScale(RectTransform targetIcon, float dropSize)
        {
            float scaleFactor = _cachedIconHeights[targetIcon] / dropSize;
            return new Vector3(scaleFactor, scaleFactor, 1f);
        }

        /// <summary>
        /// Универсальный метод получения экранной позиции UI элемента
        /// </summary>
        private Vector3 GetUIScreenPosition(RectTransform rect)
        {
            // Если Canvas в режиме Screen Space - Overlay
            if (_canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return rect.position; // В этом режиме position уже в пикселях экрана
            }
            
            // Если Canvas в режиме Screen Space - Camera
            return RectTransformUtility.WorldToScreenPoint(_mainCam, rect.position);
        }

    }
}
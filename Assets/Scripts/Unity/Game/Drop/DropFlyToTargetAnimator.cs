using System;
using DG.Tweening;
using UnityEngine;

namespace Unity.Game
{
    public class DropFlyToTargetAnimator : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        [Header("Ссылки")]
        [SerializeField] private Camera mainCam;
        [SerializeField] private RectTransform targetIcon; // Иконка монетки в Canvas

        [Header("Параметры полета")]
        [SerializeField] private float flightDuration = 0.6f;
        [SerializeField] private float arcHeight = 2.5f;   // Высота дуги полета
        [SerializeField] private float baseDropSize = 1f;  // Базовый размер дропа в мире (высота спрайта)

        [Header("Визуал (Squash & Stretch)")]
        [SerializeField] private Vector2 flightStretch = new Vector2(0.8f, 1.2f);
        [SerializeField] private Vector2 finalSquash = new Vector2(1.4f, 0.6f);

        private Vector3 initialScale;

        private void Awake()
        {
            if (mainCam == null) mainCam = Camera.main;
            initialScale = transform.localScale;
        }

        /// <summary>
        /// Запускает полет дропа в UI иконку
        /// </summary>
        public void FlyToIcon(Transform flyingObject, float delay, Action<Transform> onComplete)
        {
            // 1. Получаем экранные координаты иконки (в пикселях)
            Vector3 iconScreenPos = GetUIScreenPosition(targetIcon);

            // 2. Конвертируем экранные координаты в мировые координаты на глубине дропа
            // Z для ScreenToWorldPoint - это дистанция от камеры до объекта
            float zDistance = Mathf.Abs(mainCam.transform.position.z - transform.position.z);
            Vector3 targetWorldPos = mainCam.ScreenToWorldPoint(new Vector3(iconScreenPos.x, iconScreenPos.y, zDistance));

            // 3. Рассчитываем целевой масштаб (чтобы дроп стал ровно размером с иконку)
            Vector3 targetScale = CalculateTargetScale();

            // 4. Настраиваем точки для кривой Безье (красивая дуга)
            Vector3 startPos = flyingObject.position;
            Vector3 midPoint = (startPos + targetWorldPos) / 2f + Vector3.up * arcHeight;

            // Убиваем старые твины
            DOTween.Kill(flyingObject);
            
            // Сбрасываем состояние перед полетом
            flyingObject.localScale = initialScale;
            flyingObject.rotation = Quaternion.identity;

            var seq = DOTween.Sequence();

            seq.Append(
                DOTween.To(
                        () => 0f,
                        t => UpdateFlight(flyingObject, t, startPos, midPoint, targetWorldPos, targetScale),
                        1f,
                        flightDuration
                    )
                    .SetEase(Ease.InBack) // InBack дает эффект "всасывания" в UI в конце
                    .OnComplete(() =>
                    {
                        // Финальный сочный Squash перед исчезновением
                        PlayFinalSquash(flyingObject, onComplete);
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
            flyingObject.localScale = Vector3.Lerp(initialScale, targetScale, t);

            // --- ПОВОРОТ (Look at direction) ---
            // Чтобы дроп летел "носиком" вперед по траектории
            if (t < 0.99f)
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
            }
        }

        /// <summary>
        /// Рассчитывает, во сколько раз нужно увеличить/уменьшить дроп, 
        /// чтобы он визуально совпал с UI иконкой.
        /// </summary>
        private Vector3 CalculateTargetScale()
        {
            // Для Orthographic камеры (стандарт для 2D)
            if (mainCam.orthographic)
            {
                // Сколько мировых юнитов занимает 1 пиксель на экране
                float pixelToWorld = (mainCam.orthographicSize * 2f) / Screen.height;
                
                // Реальный размер иконки в пикселях (с учетом её lossyScale)
                float iconPixelHeight = targetIcon.rect.height * targetIcon.lossyScale.y;
                
                // Переводим размер иконки в мировые юниты
                float iconWorldHeight = iconPixelHeight * pixelToWorld;
                
                // Вычисляем итоговый множитель масштаба
                float scaleFactor = iconWorldHeight / baseDropSize;
                return new Vector3(scaleFactor, scaleFactor, 1f);
            }
            
            // Если камера перспективная, расчет сложнее (зависит от дистанции), 
            // но для 2D игр обычно используется блок выше.
            Debug.LogWarning("Перспективная камера не поддерживается в этом упрощенном расчете!");
            return Vector3.one;
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
            return RectTransformUtility.WorldToScreenPoint(mainCam, rect.position);
        }

        private void PlayFinalSquash(Transform target, System.Action<Transform> onComplete)
        {
            target.DOScale(new Vector3(finalSquash.x * transform.localScale.x, finalSquash.y * transform.localScale.y, 1f), 0.05f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    onComplete?.Invoke(target);
                });
        }
    }
}
using System.Collections;
using DG.Tweening;
using Unity.Infrastructure.Effects;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game.Projectile
{
    public class ProjectileBounce : ProjectileBase
    {
       
        
        [Header("Bounce")]
        [SerializeField] private int _bounceCount = 4;
        [SerializeField] private float _bounceDistance = 1.2f;
        [SerializeField] private float _bounceHeight = 0.4f;
        [SerializeField] private float _bounceDuration = 0.18f;


        protected override void OnFlightComplete()
        {
            _effectsSpawner.SpawnEffect(VisualEffectType.MuzzleCannon, transform.position, transform.parent);
            Vector3 direction = _direction * -1;

            Sequence sequence = DOTween.Sequence();

            Vector3 currentPos = transform.position;

            float distance = _bounceDistance;
            float height = _bounceHeight;
            float duration = _bounceDuration;

            int bounceCount = Random.Range(
                Mathf.Max(1, _bounceCount - 2),
                _bounceCount + 1);

            for (int i = 0; i < bounceCount; i++)
            {
                // Разброс по горизонтали и немного вверх/вниз
                direction = Quaternion.Euler(
                    Random.Range(-10f, 10f),   // Pitch
                    Random.Range(-20f, 20f),   // Yaw
                    0f) * direction;

                Vector3 nextPos = currentPos + direction * distance;

                float spin = Random.Range(220f, 520f);
                if (Random.value > 0.5f)
                    spin = -spin;

                sequence.Append(
                    transform.DOJump(nextPos, height, 1, duration)
                        .SetEase(Ease.OutQuad));

               

                currentPos = nextPos;

                // Затухание
                distance *= Random.Range(0.45f, 0.65f);
                height *= Random.Range(0.45f, 0.65f);
                duration *= Random.Range(0.90f, 1.05f);
            }

            /*sequence.Append(
                transform.DOScale(Vector3.zero, 0.12f)
                    .SetEase(Ease.InBack));*/

            sequence.OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }
    }
}
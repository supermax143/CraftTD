using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unity.Game
{
    public class DropAnimator : MonoBehaviour
    {
        
        [SerializeField]
        private float _minDistance = 1.5f;
        [SerializeField]
        private float _maxDistance = 2f;
        [SerializeField]
        private float _bounceDuration = 0.5f;
        
        
        public void Show( Transform target, Vector2 direction = default, Action<Transform> onComplete = null)
        {
            if (direction == default || direction == Vector2.zero)
            {
                float angle = Random.Range(0f, 360f);
                direction = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad)
                );
            }
            
            Vector2 offset = direction * Random.Range(_minDistance, _maxDistance);
            int bounces = (int)Mathf.Round(offset.magnitude) + 1;
            
            Sequence moveSeq = DOTween.Sequence();

            Vector3 startPos = target.position;
            
            // Горизонтальное затухающее движение
            moveSeq.Join( target.DOMoveX(startPos.x + offset.x, _bounceDuration * bounces) );

            
            float bounceDuration = _bounceDuration;
            float startJumpHeight = 2f;
            Sequence jumpSeq = DOTween.Sequence();
            float baseYStep = offset.y/bounces;
            float currentPosition = target.position.y;
            for (int i = 0; i < bounces; i++)
            {
                JumpSeq(target, startJumpHeight, i, jumpSeq, baseYStep, bounceDuration, ref currentPosition);
            }
            moveSeq.onComplete += () =>
            {
                onComplete?.Invoke(target);
                UpdateSortingByPosition(target);
            };
            
            
        }

        public void UpdateSortingByPosition(Transform target)
        {
            var pos = target.position;
            pos.z = pos.y * 0.001f;
            target.position = pos;
        }
        
        private void JumpSeq(Transform target, float height, int i, Sequence seq, float yStep, float bounceDuration, ref float currentBaseY)
        {
            float jumpHeight = height * Mathf.Pow(0.5f, i);
            float baseYStep = yStep * Mathf.Pow(0.5f, i);
            
            // Вверх
            seq.Append(
                target.DOMoveY(currentBaseY + jumpHeight, bounceDuration * 0.4f)
                    .SetEase(Ease.OutQuad)
            );

            if (i == 0)
            {
                seq.Join(
                    target.DOScale(1, bounceDuration * 0.4f)
                );
            }
            
            
            // Вниз
            currentBaseY += baseYStep;
            seq.Append(
                target.DOMoveY(currentBaseY, bounceDuration * 0.6f)
                    .SetEase(Ease.InQuad)
            );

            // Squash при касании земли
            seq.AppendCallback(() =>
            {
                target.DOKill(false);
                UpdateSortingByPosition(target);
                Sequence squash = DOTween.Sequence();

                squash.Append(
                    target.DOScale(
                        new Vector3(1.15f, 0.85f, 1),
                        0.05f)
                );

                squash.Append(
                    target.DOScale(Vector3.one, 0.1f)
                        .SetEase(Ease.OutBack)
                );
            });
        }
    }
}
using DG.Tweening;
using UnityEngine;

namespace Unity.Game
{
    public class DropManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _rewardMoneyView;
        
        [Header("Горизонтальное движение (X)")]
        public float _minDistanceX = 1.5f;
        public float _maxDistanceX = 2f;
        public float _totalDuration = 1.5f;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ShowDrop(inputPosition);
            }
        }

        public void ShowDrop(Vector2 position)
        {
            GameObject rewardView = Instantiate(_rewardMoneyView, position, Quaternion.identity);
            var drop = rewardView.transform;

            drop.localScale = Vector3.one * .5f;

            float angle = Random.Range(0f, 360f);
            
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );
            Vector2 randomOffset = direction * Random.Range(_minDistanceX, _maxDistanceX);

            
            Play(
                drop,
                randomOffset,
                bounces: Random.Range(2, 4));
        }
        
        public void Play(
            Transform target,
            Vector2 offset,
            int bounces = 3)
        {
            Sequence moveSeq = DOTween.Sequence();

            Vector3 startPos = target.position;

            // Горизонтальное затухающее движение
            moveSeq.Join(
                target.DOMoveX(startPos.x + offset.x, _totalDuration)
                    //.SetEase(Ease.OutCubic)
                );

            float bounceDuration = _totalDuration / bounces;
            float startJumpHeight = 2f;
            Sequence jumpSeq = DOTween.Sequence();
            float baseYStep = offset.y/bounces;
            float currentPosition = target.position.y;
            Debug.Log(baseYStep);
            for (int i = 0; i < bounces; i++)
            {
                JumpSeq(target, startJumpHeight, i, jumpSeq, baseYStep, bounceDuration, ref currentPosition);
            }

        }

        private static void JumpSeq(Transform target, float height, int i, Sequence seq, float yStep, float bounceDuration, ref float currentBaseY)
        {
            float jumpHeight = height * Mathf.Pow(0.5f, i);
            float baseYStep = yStep * Mathf.Pow(0.5f, i);
            
            // Вверх
            seq.Append(
                target.DOMoveY(currentBaseY + jumpHeight, bounceDuration * 0.4f)
                    .SetEase(Ease.OutQuad)
            );

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
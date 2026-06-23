using DG.Tweening;
using UnityEngine;

namespace Unity.Game
{
    public class DropManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _rewardMoneyView;
        
        [SerializeField] private float minLaunchForce = 2f;
        [SerializeField] private float maxLaunchForce = 4f;
        
        [SerializeField]
        private AnimationCurve _curveY;
        
        [Header("Скейл (вылет)")]
        public float overshootScale = 1.3f;
        public float finalScale = 1f;
        public float popDuration = 0.18f;

        [Header("Горизонтальное движение (X)")]
        public float minDistanceX = 0.4f;
        public float maxDistanceX = 1.0f;
        public float moveDuration = 0.6f;

        [Header("Баунсы по Y")]
        public float firstBounceHeight = 0.6f;
        public int bounceCount = 4;
        public float bounceDecay = 0.5f;   // во сколько раз уменьшается высота каждого след. прыжка
        public float bounceDurationDecay = 0.55f; // во сколько раз уменьшается время каждого след. прыжка
        public float firstBounceDuration = 0.25f;

        [Header("Squash на ударе")]
        /*public float squashAmount = 0.2f;
        public float squashDuration = 0.08f;*/
        [Header("удары")]
        [SerializeField]
        private Ease _scaleEase = Ease.InBounce;
        
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
            var startScale = _rewardMoneyView.transform.localScale;


            var drop = rewardView.transform;


            drop.localScale = Vector3.one * .5f;

            float angle = Random.Range(0f, 360f);
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );
            Vector2 randomOffset = direction * Random.Range(minDistanceX, maxDistanceX);

            
            Play(
                drop,
                randomOffset,
                bounces: 4,
                totalDuration: 1.5f
            );
        }
        
        public static Sequence Play(
            Transform target,
            Vector2 horizontalOffset,
            int bounces = 3,
            float totalDuration = 1.2f)
        {
            Sequence moveSeq = DOTween.Sequence();

            Vector3 startPos = target.position;

            // Горизонтальное затухающее движение
            moveSeq.Join(
                target.DOMoveX(startPos.x + horizontalOffset.x, totalDuration)
                    .SetEase(Ease.OutCubic)
                );

            float bounceDuration = totalDuration / bounces;
            float height = 2f;
            Sequence jumpSeq = DOTween.Sequence();
            for (int i = 0; i < bounces; i++)
            {
                JumpSeq(target, height, i, jumpSeq, startPos, bounceDuration);
            }

            return moveSeq;
        }

        private static void JumpSeq(Transform target, float height, int i, Sequence seq, Vector3 startPos, float bounceDuration)
        {
            float jumpHeight = height * Mathf.Pow(0.5f, i);

            
            
            // Вверх
            seq.Append(
                target.DOMoveY(startPos.y + jumpHeight, bounceDuration * 0.4f)
                    .SetEase(Ease.OutQuad)
            );

            // Вниз
            seq.Append(
                target.DOMoveY(startPos.y, bounceDuration * 0.6f)
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
using System;
using DG.Tweening;
using Unity.Game.Attributes.Specific;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Unity.Game
{
    /// <summary>
    /// Компонент для управления наградами за убийство юнита
    /// </summary>
    public class RewardComponent : GameComponent
    {
        [SerializeField]
        private RewardMoneyAttribute _rewardMoney = new RewardMoneyAttribute(0);
        [SerializeField]
        private GameObject _rewardMoneyView;
        
        [Inject] ILevelRewardAggregator _rewardAggregator;
        
        
        private HealthComponent _healthComponent;

        public int RewardMoney => _rewardMoney.ValueModified;

        public void Initialize(HealthComponent healthComponent)
        {
            _healthComponent = healthComponent;
            _healthComponent.OnDeath += OnDeathHandler;
        }

        private void OnDeathHandler()
        {
            _rewardAggregator.AddMoney(RewardMoney);
            
            GameObject rewardView = Instantiate(_rewardMoneyView, transform.position, Quaternion.identity);
            var startScale = _rewardMoneyView.transform.localScale;


            var drop = rewardView.transform;
            /*if (_rewardMoneyView != null)
            {
                rewardView.transform.DOScale(Vector3.one * 1.5f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    rewardView.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutQuad);
                });
            }*/
           
            drop.localScale = Vector3.zero;

            transform.localScale = Vector3.zero;

            float dirX = Random.value > 0.5f ? 1f : -1f;
            float distX = Random.Range(minDistanceX, maxDistanceX);
            float groundY = drop.position.y; // уровень "пола" в 2D
            float targetX = drop.position.x + dirX * distX;

            Sequence seq = DOTween.Sequence();

            // 1. Вылет — скейл 0 -> 1.3, движение по X стартует параллельно
            seq.Append(drop.DOScale(overshootScale, popDuration).SetEase(Ease.OutQuad));
            seq.Join(drop.DOMoveX(targetX, moveDuration).SetEase(Ease.OutCubic));

            // 2. Возврат скейла к финальному размеру
            seq.Append(drop.DOScale(finalScale, 0.12f).SetEase(Ease.OutQuad));

            // 3. Цикл затухающих прыжков по Y
            float height = firstBounceHeight;
            float upDuration = firstBounceDuration;
            float downDuration = firstBounceDuration * 1.1f;

            for (int i = 0; i < bounceCount; i++)
            {
                float peakY = groundY + height;

                // взлёт
                seq.Append(drop.DOMoveY(peakY, upDuration).SetEase(Ease.OutQuad));

                // в момент пика — лёгкое вытягивание (stretch), опционально
                seq.Join(drop.DOScale(finalScale * (1f + squashAmount * 0.3f), upDuration).SetEase(Ease.OutQuad));

                // падение
                seq.Append(drop.DOMoveY(groundY, downDuration).SetEase(Ease.InQuad));

                // squash в момент удара о "пол"
                seq.Join(DOTween.Sequence()
                    .Append(drop.DOScale(new Vector3(finalScale * (1f + squashAmount), finalScale * (1f - squashAmount), 1f), squashDuration))
                    .Append(drop.DOScale(Vector3.one * finalScale, squashDuration)));

                // уменьшаем параметры для следующего прыжка
                height *= bounceDecay;
                upDuration *= bounceDurationDecay;
                downDuration *= bounceDurationDecay;
            }
            
        }

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
        public float squashAmount = 0.2f;
        public float squashDuration = 0.08f;
        
        
        
        private void OnDestroy()
        {
            _healthComponent.OnDeath -= OnDeathHandler;
        }
    }
}

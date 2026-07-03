using DG.Tweening;
using Unity.Utils;
using UnityEngine;

namespace Unity.Game
{
    public class DeathState : UnitState
    {
        public override void Enter()
        {
            _unit.View.StartIdle();
            Die(_unit.AttackTarget.LastAttackDirection);
        }
        
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform visual;

        [Header("Knockback")]
        [SerializeField] private float knockbackForce = 7f;

        public void Die(Vector2 hitDirection)
        {
            //_unit.gameObject.SetLayerRecursively(Layers.Dead);
            // Включаем физику
            rb.bodyType = RigidbodyType2D.Dynamic;
            // Отбрасываем
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);

            PlayBounceAnimation(hitDirection);
        }

        private void PlayBounceAnimation(Vector2 direction)
        {
            visual.DOKill();

            float sign = Mathf.Sign(direction.x);

            Sequence seq = DOTween.Sequence();

            // Первый большой прыжок
            seq.Append(visual.DOLocalMoveY(0.35f, 0.12f).SetEase(Ease.OutQuad));
            seq.Append(visual.DOLocalMoveY(0f, 0.12f).SetEase(Ease.InQuad));

            // Второй
            seq.Append(visual.DOLocalMoveY(0.18f, 0.09f).SetEase(Ease.OutQuad));
            seq.Append(visual.DOLocalMoveY(0f, 0.09f).SetEase(Ease.InQuad));

            // Третий
            seq.Append(visual.DOLocalMoveY(0.08f, 0.07f).SetEase(Ease.OutQuad));
            seq.Append(visual.DOLocalMoveY(0f, 0.07f).SetEase(Ease.InQuad));

            // Одновременно вращение
            visual.DOLocalRotate(
                    new Vector3(0, 0, sign * 40f),
                    0.6f,
                    RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic);

            // Немного "расплющить" при первом ударе
            seq.Insert(
                0.12f,
                visual.DOScale(new Vector3(1.08f, 0.92f, 1), 0.05f)
                    .SetLoops(2, LoopType.Yoyo));

            seq.OnComplete(() =>
            {
                visual.localPosition = Vector3.zero;
                _unit.Die();
            });
        }
        
    }
}
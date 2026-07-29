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
            _unit.gameObject.SetLayerRecursively(Layers.Dead);
            _unit.HealthComponent.HideHealth();
            _unit.View.StartDie();
            // Включаем физику
            rb.bodyType = RigidbodyType2D.Dynamic;
            // Отбрасываем
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);

            DOVirtual.DelayedCall(1.5f, () => _unit.Die(), false);
        }

        public override void UpdateState()
        {
            _unit.View.UpdateSortingByPosition();
        }
        
    }
}
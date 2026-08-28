using System.Linq;
using Unity.Game.Attributes.Specific;
using Unity.Infrastructure.Effects;
using UnityEngine;
using Zenject;

namespace Unity.Game.Spells.EffectsApplyer
{
    public class AOEHealEffectApplier : SpellEffectApplier
    {
        [SerializeField] 
        private HealthAttribute _health;
        [SerializeField] 
        private float _radius = 2;
        [SerializeField] 
        private Faction _faction = Faction.Player;
        
        [Inject] private PopupSpawnManager _popupSpawnManager;
        
        public override void Apply(SpellController spell)
        {
            foreach (var modifier in spell.Model.GetModifiers())
            {
                _health.AddModifier(modifier);
            }
            
            Vector3 position = spell.ActivationComponent.TargetPosition;
            position.z = position.y * 0.001f;
            var colliders = Physics2D.OverlapCircleAll(position, _radius);
            var targets = colliders
                .Select(c => c.GetComponent<AttackTargetBase>())
                .Where(t => t != null && !t.IsDead && t.Faction == _faction)
                .Distinct();

            foreach (var target in targets)
            {
                target.HealthComponent.Heal(_health.BaseValueModified);
            }
            _popupSpawnManager.SpawnEffect(PopupType.HealEffect, position);
        }
        
        
    }
}
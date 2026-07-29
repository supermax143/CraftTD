using System;
using Unity.Settings;
using Unity.Utils;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Unity.Game
{
    [RequireComponent(typeof(TowerView))]
    public class TowerController : GameComponent
    {

        public event Action<TowerController> OnDestroyed;
        
        [SerializeField, HideInInspector]
        private TowerView _view;
        [SerializeField, HideInInspector]
        private AttackTargetBase attackTarget;
        [SerializeField, HideInInspector]
        private HealthComponent _health;
        [SerializeField, HideInInspector]
        private RewardForDamageComponent _rewardForDamage;
        
        [Inject] private GameSettings _gameSettings;
        
        private Faction _faction;

        public Faction Faction => _faction;

        public AttackTargetBase AttackTarget => attackTarget;

        private void Start()
        {
            _health.OnDeath += OnDeath;
        }

        private void OnValidate()
        {
            _view = GetComponentInChildren<TowerView>();
            _health = GetComponentInChildren<HealthComponent>();
            attackTarget = GetComponentInChildren<AttackTargetBase>();
            _rewardForDamage = GetComponentInChildren<RewardForDamageComponent>();
        }

        public void SetFaction(Faction faction)
        {
            _faction = faction;
            attackTarget.SetFaction(_faction);
            if (_gameSettings.TryGetFactionColor(faction, out var color))
            {
                _view.SetColor(color);
            }
            var layer = _faction == Faction.Player ? Layers.PlayerTower : Layers.EnemyTower;
            gameObject.SetLayerRecursively(layer, Layers.SpawnBorder);
        }

        public override void SetData(GameEntityData data)
        {
            base.SetData(data);
            _health.SetData(data);
            attackTarget.Initialize(_health);
            if (_faction == Faction.Enemy)
            {
                _rewardForDamage.Initialize(_health);
                _rewardForDamage.SetData(data);
            }
        }

        private void OnDeath()
        {
            OnDestroyed?.Invoke(this);
            _view.ShowExplosion(Dispose);
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _health.OnDeath -= OnDeath;
        }
    }
}
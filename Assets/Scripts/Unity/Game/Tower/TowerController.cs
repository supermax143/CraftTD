using System;
using Unity.Settings;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Unity.Game
{
    [RequireComponent(typeof(TowerView))]
    public class TowerController : GameEntity
    {

        public event Action<TowerController> OnDestroyed;
        
        [SerializeField, HideInInspector]
        private TowerView _view;
        [SerializeField, HideInInspector]
        private AttackTarget _attackTarget;
        [SerializeField, HideInInspector]
        private HealthComponent _health;
        
        [Inject] private GameSettings _gameSettings;
        
        private Faction _faction;

        public Faction Faction => _faction;

        public AttackTarget AttackTarget => _attackTarget;

        private void Start()
        {
            _health.OnDeath += OnDeath;
        }

        private void OnValidate()
        {
            _view = GetComponentInChildren<TowerView>();
            _health = GetComponentInChildren<HealthComponent>();
            _attackTarget = GetComponentInChildren<AttackTarget>();
        }

        public void SetFaction(Faction faction)
        {
            _faction = faction;
            _attackTarget.SetFaction(_faction);
            if (_gameSettings.TryGetFactionColor(faction, out var color))
            {
                _view.SetColor(color);
            }
        }

        public override void SetData(GameEntityInfo info)
        {
            base.SetData(info);
            _health.SetData(info);
            _attackTarget.Initialize(_health);
        }

        private void OnDeath()
        {
            OnDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            _health.OnDeath -= OnDeath;
        }
    }
}
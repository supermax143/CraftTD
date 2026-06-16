using System;
using Unity.Settings;
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
        }

        public void SetFaction(Faction faction)
        {
            _faction = faction;
            attackTarget.SetFaction(_faction);
            if (_gameSettings.TryGetFactionColor(faction, out var color))
            {
                _view.SetColor(color);
            }
        }

        public override void SetData(GameEntityData data)
        {
            base.SetData(data);
            _health.SetData(data);
            attackTarget.Initialize(_health);
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
using System;
using System.Collections.Generic;
using Unity.Game.Attributes;
using Unity.Settings;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Unity.Game
{
    [RequireComponent(typeof(UnitView))]
    public class UnitController : GameEntity
    {
        
        [SerializeField, HideInInspector] 
        private UnitView _view;
        [SerializeField, HideInInspector]
        private AttackTarget _attackTarget;
        [SerializeField, HideInInspector]
        private UnitStateManager _stateManager;
        [SerializeField, HideInInspector]
        private HealthComponent _healthComponent;
        [SerializeField, HideInInspector]
        private AttackComponent _attackComponent;
        [SerializeField, HideInInspector]
        private MoveComponent _moveComponent;
        [SerializeField, HideInInspector]
        private TargetSearchComponent _targetSearchComponent;
       
        
        
        [Inject] private GameSettings _gameSettings;
        
        private Faction _faction;
        private Faction _opponentFaction;
        private GameEntityInfo _info;
        
        public Faction OpponentFaction => _opponentFaction;
        public AttackComponent Attack => _attackComponent;
        public HealthComponent HealthComponent => _healthComponent;
        public MoveComponent MoveComponent => _moveComponent;
        public TargetSearchComponent TargetSearchComponent => _targetSearchComponent;
        public Faction Faction => _faction;


        private void OnValidate()
        {
            _view = GetComponentInChildren<UnitView>();
            _attackTarget = GetComponentInChildren<AttackTarget>();
            _stateManager = GetComponentInChildren<UnitStateManager>();
            _healthComponent = GetComponentInChildren<HealthComponent>();
            _attackComponent = GetComponentInChildren<AttackComponent>();
            _moveComponent = GetComponentInChildren<MoveComponent>();
            _targetSearchComponent = GetComponentInChildren<TargetSearchComponent>();
        }

        
        public void SetFaction(Faction faction, Faction enemyFaction)
        {
            _faction = faction;
            _opponentFaction = enemyFaction;
            _attackTarget.SetFaction(_faction);
            _targetSearchComponent.SetFaction(_faction, _opponentFaction);
            if (!_gameSettings.TryGetFactionColor(faction, out var color))
            {
                Debug.LogError(this.GetType().Name + ": Can't find faction color " + faction.ToString());
                color = Color.purple;
            }
            _view.SetColor(color);
            
        }

        public override void SetData(GameEntityInfo info)
        {
            _info = info;
            Initialize();
        }

        private void Initialize()
        {
            _healthComponent.SetData(_info);
            _attackTarget.Initialize(HealthComponent);
            _moveComponent.SetData(_info);
            _attackComponent.SetData(_info);
            _targetSearchComponent.SetData(_info);
            
            _stateManager.Initialize(this);
            _stateManager.ChangeState<SearchTargetState>();
        }

        public override IEnumerable<GameEntityAttribute> GetAllAttributes()
        {
            return _info.GetAllAttributes();
        }
        
        public void Die()
        {
            Destroy(gameObject);
        }

    }
}

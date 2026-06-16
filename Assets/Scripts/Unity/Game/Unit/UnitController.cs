using System;
using System.Collections.Generic;
using Core.Application.Models;
using Unity.Game.Attributes;
using Unity.Settings;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Unity.Game
{
    [RequireComponent(typeof(UnitView))]
    public class UnitController : GameComponent
    {
        
        [SerializeField, HideInInspector] 
        private UnitView _view;
        [SerializeField, HideInInspector]
        private AttackTargetBase attackTarget;
        [SerializeField, HideInInspector]
        private UnitStateManager _stateManager;
        [SerializeField, HideInInspector]
        private HealthComponent _healthComponent;
        [SerializeField, HideInInspector]
        private AttackComponent _attackComponent;
        [SerializeField, HideInInspector]
        private MoveComponentBase _moveComponent;
        [SerializeField, HideInInspector]
        private TargetSearchComponentBase _targetSearchComponent;
        [SerializeField, HideInInspector]
        private RewardComponent _rewardComponent;



        [Inject] private GameSettings _gameSettings;
        [Inject] private IMainModel _mainModel;
        
        private Faction _faction;
        private Faction _opponentFaction;
        private GameEntityData _data;
        
        public Faction OpponentFaction => _opponentFaction;
        public AttackComponent Attack => _attackComponent;
        public HealthComponent HealthComponent => _healthComponent;
        public MoveComponentBase MoveComponent => _moveComponent;
        public TargetSearchComponentBase TargetSearchComponent => _targetSearchComponent;
        public Faction Faction => _faction;

        public UnitView View => _view;


        private void OnValidate()
        {
            _view = GetComponentInChildren<UnitView>();
            attackTarget = GetComponentInChildren<AttackTargetBase>();
            _stateManager = GetComponentInChildren<UnitStateManager>();
            _healthComponent = GetComponentInChildren<HealthComponent>();
            _attackComponent = GetComponentInChildren<AttackComponent>();
            _moveComponent = GetComponentInChildren<MoveComponentBase>();
            _targetSearchComponent = GetComponentInChildren<TargetSearchComponentBase>();
            _rewardComponent = GetComponentInChildren<RewardComponent>();
        }

        
        public void SetFaction(Faction faction, Faction enemyFaction)
        {
            _faction = faction;
            _opponentFaction = enemyFaction;
            attackTarget.SetFaction(_faction);
            _targetSearchComponent.SetFaction(_faction, _opponentFaction);
            if (!_gameSettings.TryGetFactionColor(faction, out var color))
            {
                Debug.LogError(this.GetType().Name + ": Can't find faction color " + faction.ToString());
                color = Color.purple;
            }
            View.SetColor(color);
            
        }

        public override void SetData(GameEntityData data)
        {
            _data = data;
            Initialize();
        }

        private void Initialize()
        {
            _healthComponent.SetData(_data);
            attackTarget.Initialize(HealthComponent);
            _moveComponent.SetData(_data);
            _attackComponent.SetData(_data);
            _targetSearchComponent.SetData(_data);
            if (_faction == Faction.Enemy)
            {
                _rewardComponent.Initialize(HealthComponent);
                _rewardComponent.SetData(_data);
            }

            //_healthComponent.OnDeath += OnDeathHandler;

            _stateManager.Initialize(this);
            _stateManager.ChangeState<SearchTargetState>();
        }

        public override IEnumerable<GameEntityAttribute> GetAllAttributes()
        {
            return _data.GetAllAttributes();
        }
        
        public void Die()
        {
            Destroy(gameObject);
        }

        /*private void OnDeathHandler()
        {
            if (_faction == Faction.Enemy)
            {
                var epoch = _mainModel.Epoch;
                epoch.Money += (uint)_rewardComponent.RewardMoney;
            }
        }*/

        /*private void OnDestroy()
        {
            if (_healthComponent != null)
            {
                _healthComponent.OnDeath -= OnDeathHandler;
            }
        }*/

    }
}

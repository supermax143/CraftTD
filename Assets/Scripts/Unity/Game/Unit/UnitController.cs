using System;
using System.Collections.Generic;
using Core.Application.Info.Attributes.AttributeModifiers;
using Core.Application.Models;
using Environments.Common.Scripts;
using Unity.Game.Attributes;
using Unity.Settings;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Unity.Game
{
    public class UnitController : GameComponent, ITouchTarget
    {
        public event Action<UnitController> OnDie;
        public event Action<ITouchTarget, Vector2> OnClick;
        
        [SerializeField, HideInInspector] 
        private UnitView _view;
        [SerializeField, HideInInspector]
        private AttackTargetBase _attackTarget;
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
        public AttackTargetBase AttackTarget => _attackTarget;
        public RewardComponent RewardComponent1 => _rewardComponent;
        public Faction Faction => _faction;
        public UnitModel Model { get; private set; }

        public UnitView View => _view;



        private void OnValidate()
        {
            _view = GetComponentInChildren<UnitView>();
            _attackTarget = GetComponentInChildren<AttackTargetBase>();
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
            _attackTarget.SetFaction(_faction);
            _targetSearchComponent.SetFaction(_faction, _opponentFaction);
            if (!_gameSettings.TryGetFactionColor(faction, out var color))
            {
                Debug.LogError(this.GetType().Name + ": Can't find faction color " + faction.ToString());
                color = Color.purple;
            }
            View.SetColor(color);
            
        }

        public void SetModel(UnitModel model)
        {
            Model = model;
            SetData(model.Entity);
        }


        public override void SetData(GameEntityData data)
        {
            _data = data;
            Initialize();
        }

        private void Initialize()
        {
            _healthComponent.SetData(_data);
            _view.Initialize(_healthComponent, transform);
            _attackTarget.Initialize(_healthComponent);
            _moveComponent.SetData(_data);
            _attackComponent.SetData( _data);
            _attackComponent.Initialize(_view, _moveComponent);
            _targetSearchComponent.SetData(_data);
            _targetSearchComponent.Initialize(_moveComponent);
            if (_faction == Faction.Enemy)
            {
                _rewardComponent.Initialize(_healthComponent);
                _rewardComponent.SetData(_data);
            }

            _stateManager.Initialize(this);
            _stateManager.ChangeState<SearchTargetState>();
        }

        public void ApplyModifiers(IEnumerable<AttributeModifierBase> modifiers)
        {
            _healthComponent.ApplyAttributeModifiers(modifiers);
            _attackComponent.ApplyAttributeModifiers(modifiers);
            _moveComponent.ApplyAttributeModifiers(modifiers);
            _targetSearchComponent.ApplyAttributeModifiers(modifiers);
            _rewardComponent.ApplyAttributeModifiers(modifiers);
        }
        
        public override IEnumerable<GameEntityAttribute> GetAllAttributes()
        {
            return _data.GetAllAttributes();
        }
        
        public void Die()
        {
            OnDie?.Invoke(this);
            if (_faction == Faction.Enemy)
            {
                _rewardComponent.OnDeathHandler();
            }
            Dispose();
        }

        
        
        public void Dispose()
        {
            Destroy(gameObject);
        }
        
        public void HandleClick(Vector2 touchPosition)
        {
            OnClick?.Invoke(this, touchPosition);
        }
    }
}

using System;
using Unity.Settings;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    [RequireComponent(typeof(UnitView))]
    public class UnitController : MonoBehaviour
    {
        [SerializeField] 
        private UnitAttack _attack;
        [SerializeField, HideInInspector] 
        private UnitView _view;
        [SerializeField, HideInInspector]
        private AttackTarget _attackTarget;
        [SerializeField, HideInInspector]
        private UnitStateManager _stateManager;
        [SerializeField, HideInInspector]
        private HealthComponent _health;
        [SerializeField]
        private float _moveSpeed = 1;
        
        [Inject] private GameSettings _gameSettings;
        
        private Faction _faction;
        private Faction _opponentFaction;
        
        public UnitAttack Attack => _attack;
        public Faction OpponentFaction => _opponentFaction;
        public float MoveSpeed => _moveSpeed;

        private void OnValidate()
        {
            _view = GetComponentInChildren<UnitView>();
            _attackTarget = GetComponentInChildren<AttackTarget>();
            _stateManager = GetComponentInChildren<UnitStateManager>();
            _health = GetComponentInChildren<HealthComponent>();
        }

        private void Start()
        {
            Initialize();
        }

        public void SetFaction(Faction faction, Faction enemyFaction)
        {
            _faction = faction;
            _opponentFaction = enemyFaction;
            _attackTarget.SetFaction(_faction);
            if (!_gameSettings.TryGetFactionColor(faction, out var color))
            {
                Debug.Log(this.GetType().Name + ": Can't find faction color " + faction.ToString());
                color = Color.purple;
            }
            _view.SetColor(color);
            
        }

        public void Initialize()
        {
            _health.Initialize();
            _health.OnDeath += OnDeath;
            _stateManager.Initialize(this);
            _stateManager.ChangeState<MoveToTowerState>();
            _attackTarget.Initialize(_health);
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}

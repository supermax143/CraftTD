using System;
using Unity.Settings;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Unity.Game
{
    [RequireComponent(typeof(UnitView))]
    public class UnitController : MonoBehaviour
    {
        [FormerlySerializedAs("_attack")] [SerializeField] 
        private AttackData attackData;
        [SerializeField, HideInInspector] 
        private UnitView _view;
        [SerializeField, HideInInspector]
        private AttackTarget _attackTarget;
        [SerializeField, HideInInspector]
        private UnitStateManager _stateManager;
        [SerializeField, HideInInspector]
        private HealthComponent _health;
        [SerializeField, HideInInspector]
        private AttackComponent _attackComponent;
        
        [SerializeField]
        private float _moveSpeed = 1;
        [SerializeField] 
        private float _detectionRange;
        [SerializeField] 
        private float _detectionInterval;
        
        [Inject] private GameSettings _gameSettings;
        
        private Faction _faction;
        private Faction _opponentFaction;
        
        public Faction OpponentFaction => _opponentFaction;
        public float MoveSpeed => _moveSpeed;
        public float DetectionRange => _detectionRange;
        public float DetectionInterval => _detectionInterval;

        public AttackComponent Attack => _attackComponent;

        public HealthComponent Health => _health;

        private void OnValidate()
        {
            _view = GetComponentInChildren<UnitView>();
            _attackTarget = GetComponentInChildren<AttackTarget>();
            _stateManager = GetComponentInChildren<UnitStateManager>();
            _health = GetComponentInChildren<HealthComponent>();
            _attackComponent = GetComponentInChildren<AttackComponent>();
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
                Debug.LogError(this.GetType().Name + ": Can't find faction color " + faction.ToString());
                color = Color.purple;
            }
            _view.SetColor(color);
            
        }

        public void Initialize()
        {
            Health.Initialize();
            _stateManager.Initialize(this);
            _stateManager.ChangeState<MoveToTowerState>();
            _attackTarget.Initialize(Health);
            _attackComponent.Initialize(attackData.Clone());
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}

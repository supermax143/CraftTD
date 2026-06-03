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
        [SerializeField, HideInInspector]
        private MoveComponent _move;
        [SerializeField, HideInInspector]
        private TargetSearchComponent _targetSearch;
        
        [SerializeField]
        private AttributesData _data;
        
        [SerializeField] 
        private AttackData _attackData;
        [SerializeField] 
        private MoveData _moveData;
        [SerializeField] 
        private TargetSearchData _searchData;
        
        
        [Inject] private GameSettings _gameSettings;
        
        private Faction _faction;
        private Faction _opponentFaction;
        
        public Faction OpponentFaction => _opponentFaction;
        public AttackComponent Attack => _attackComponent;
        public HealthComponent Health => _health;
        public MoveComponent Move => _move;
        public TargetSearchComponent TargetSearch => _targetSearch;


        private void OnValidate()
        {
            _view = GetComponentInChildren<UnitView>();
            _attackTarget = GetComponentInChildren<AttackTarget>();
            _stateManager = GetComponentInChildren<UnitStateManager>();
            _health = GetComponentInChildren<HealthComponent>();
            _attackComponent = GetComponentInChildren<AttackComponent>();
            _move = GetComponentInChildren<MoveComponent>();
            _targetSearch = GetComponentInChildren<TargetSearchComponent>();
        }

        /*private void Start()
        {
            Initialize();
        }*/

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
            _health.Initialize();
            _attackTarget.Initialize(Health);
            _move.Initialize(_moveData.Clone());
            _targetSearch.Initialize(_searchData.Clone(), this);
            _attackComponent.Initialize(_attackData.Clone(), this);
            _stateManager.Initialize(this);
            _stateManager.ChangeState<SearchTargetState>();
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}

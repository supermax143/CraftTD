using System;
using System.Collections.Generic;
using Core.Application.Models;
using Unity.Utils.Time;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class Team : MonoBehaviour
    {
        public event Action<TowerController> OnTowerDestroyed;
        
        [SerializeField, HideInInspector] 
        private Spawner _spawner;
        [SerializeField] 
        private Transform _towerPlaceholder;
        
        [SerializeField] 
        private Faction _faction;
        [SerializeField] 
        private Faction _enemyFaction;
        
        [Inject] IMainModel _model;
        [Inject] private DiContainer _container;

        public EpochModel Epoch => _faction == Faction.Player ? _model.PlayerEpoch : _model.EnemyEpoch;
        
        private readonly List<UnitController> _units = new();
        
        public TowerController Tower => _tower;
        
        public Spawner Spawner => _spawner;

        public Faction Faction => _faction;

        private TowerController _tower;

        private void OnValidate()
        {
            _spawner = GetComponentInChildren<Spawner>();
        }

        private void Awake()
        {
            _spawner.OnUnitSpawned += OnUnitSpawned;
        }

        private void OnUnitSpawned(UnitController unit)
        {
            unit.OnDie += OnUnitDie;
            _units.Add(unit);
        }

        private void OnUnitDie(UnitController unit)
        {
            unit.OnDie -= OnUnitDie;
            _units.Remove(unit);
        }

        public void Initialize()
        {
            _tower = _container.InstantiatePrefabForComponent<TowerController>( Epoch.Info.Tower.TowerPrefab,_towerPlaceholder);
            _tower.OnDestroyed += TowerDestroyedHandler;
            _tower.SetFaction(_faction);
            _tower.SetData(Epoch.Tower.Entity);
            _spawner.SetFaction(_faction, _enemyFaction);
        }

        private void TowerDestroyedHandler(TowerController tower)
        {
            OnTowerDestroyed?.Invoke(tower);
        }

        public void StartGame()
        {
            _spawner.StartSpawn(Epoch);
        }

        public void Reset()
        {
            Debug.Log($"{this.GetType().Name} Reset");
            if (_tower != null)
            {
                _tower.OnDestroyed -= TowerDestroyedHandler;
                _tower.Dispose();
            }
            while (_units.Count > 0)
            {
                var unit = _units[0];
                OnUnitDie(unit);
                unit.Dispose();
            }
            _spawner.Reset();
            Initialize();
        }
    }
}
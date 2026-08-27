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

        public bool TowerDestroyed => _towerDestroyed;

        public List<UnitController> Units => _units;

        private TowerController _tower;
        private TowerController _newTower;

        private bool _towerDestroyed = false;
        
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

        public void InstantiateAndShowNewTower(GameObject prefab, float time, bool inversed)
        {
            _newTower = _container.InstantiatePrefabForComponent<TowerController>(prefab, _towerPlaceholder); 
            _newTower.Show(time, inversed);
        }
        
        public void UpdateView()
        {
            if (_newTower != null)
            {
                _tower = _newTower;
                _newTower = null;
            }
            else
            {
                _tower = _container.InstantiatePrefabForComponent<TowerController>( Epoch.Info.Tower.TowerPrefab,_towerPlaceholder);
            }
            _tower.OnDestroyed += TowerDestroyedHandler;
            _tower.SetFaction(_faction);
            _tower.SetData(Epoch.Tower.Entity);
            _spawner.SetFaction(_faction, _enemyFaction);
            _towerDestroyed = false;
        }

        private void TowerDestroyedHandler(TowerController tower)
        {
            _towerDestroyed = true;
            OnTowerDestroyed?.Invoke(tower);
        }

        public void StartGame()
        {
            _spawner.StartSpawn(Epoch);
        }

        public void HideAll(float time, bool inversed)
        {
            HideTower(time, inversed);
            HideUnits(time, inversed);
        }

        public void HideTower(float time, bool inversed)
        {
            if (_towerDestroyed)
            {
                return;
            }
            _tower.Hide(time, inversed);
        }
        
        public void HideUnits(float time, bool inversed)
        {
            foreach (var unit in _units)
            {
                unit.View.Hide(time, inversed);
            }
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
            _towerDestroyed = false;
            UpdateView();
        }
    }
}
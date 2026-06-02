using System;
using System.Collections.Generic;
using Unity.Utils.Time;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Game
{
    public class Team : MonoBehaviour
    {
        [SerializeField] 
        private TowerController _tower;
        [SerializeField] 
        private Spawner _spawner;
        [SerializeField]
        private float _spawnDelay = 1f;
        [SerializeField] 
        private Faction _faction;
        [SerializeField] 
        private Faction _enemyFaction;
        
        
        public TowerController Tower => _tower;
    
        private Timer _spawnTimer = new();
        
        private readonly List<UnitController> _units = new();

        private void Start()
        {
            _tower.SetFaction(_faction);
            _spawnTimer.Complete += OnSpawnTimerComplete;
            _spawnTimer.Start(_spawnDelay);
            OnSpawnTimerComplete();
        }

        private void Update()
        {
            _spawnTimer.Update();
        }

        private void OnSpawnTimerComplete()
        {
            Debug.Log("OnSpawnTimerComplete");
            _units.Add(_spawner.Spawn(_faction, _enemyFaction));
            _spawnTimer.Start(_spawnDelay);
        }


        
        
        
    }
}
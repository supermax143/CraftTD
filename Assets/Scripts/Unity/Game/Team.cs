using System;
using System.Collections.Generic;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game
{
    public class Team : MonoBehaviour
    {
        [SerializeField] 
        private Tower _tower;
        [SerializeField] 
        private Spawner _spawner;
        [SerializeField]
        private float _spawnDelay = 1f;

        
        public Tower Tower => _tower;
        public Spawner Spawner => _spawner;
    
        private Timer _spawnTimer = new();
        private readonly List<UnitActor> _units = new();

        private void Start()
        {
            _spawnTimer.Complete += OnSpawnTimerComplete;
            _spawnTimer.Start(_spawnDelay);
        }

        private void Update()
        {
            _spawnTimer.Update();
        }

        private void OnSpawnTimerComplete()
        {
            Debug.Log("OnSpawnTimerComplete");
            _units.Add(_spawner.Spawn());
            _spawnTimer.Start(_spawnDelay);
        }


        
        
        
    }
}
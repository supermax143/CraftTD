using System;
using System.Collections.Generic;
using Unity.Utils.Time;
using UnityEngine;

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
        private EpochData _epoch;

        public void Initialize(EpochData epoch)
        {
            _epoch = epoch;
            _tower.SetFaction(_faction);
            _spawner.SetFaction(_faction, _enemyFaction);
            _tower.SetData(_epoch.Tower);
        }

        public void StartGame()
        {
            _spawner.StartSpawn(_spawnDelay);
        }
        
    }
}
using System;
using System.Collections.Generic;
using Unity.Utils.Time;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class Team : MonoBehaviour
    {
        [SerializeField, HideInInspector] 
        private TowerController _tower;
        [SerializeField, HideInInspector] 
        private SpawnerBase _spawner;
        
        
        [SerializeField]
        private float _spawnDelay = 1f;
        [SerializeField] 
        private Faction _faction;
        [SerializeField] 
        private Faction _enemyFaction;
        
        
        [Inject] protected ChronologyData _chronologyData;
        
        public TowerController Tower => _tower;
        private EpochData _epoch;

        private void OnValidate()
        {
            _spawner = GetComponentInChildren<SpawnerBase>();
            _tower = GetComponentInChildren<TowerController>();
        }

        public void Initialize(EpochData epoch)
        {
            _epoch = epoch;
            _tower.SetFaction(_faction);
            _spawner.SetFaction(_faction, _enemyFaction);
            _tower.SetData(_epoch.Tower);
        }

        public void StartGame()
        {
            _spawner.StartSpawn(_chronologyData.Epochs[0]);
        }
        
    }
}
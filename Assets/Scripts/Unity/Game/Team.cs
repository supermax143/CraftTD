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
        private Spawner _spawner;
        
        
        [SerializeField] 
        private Faction _faction;
        [SerializeField] 
        private Faction _enemyFaction;
        
        [Inject] protected EpochManager _epochManager;
        
        public TowerController Tower => _tower;
        private EpochData _epoch;
        
        public Spawner Spawner => _spawner;

        public Faction Faction => _faction;


        private void OnValidate()
        {
            _spawner = GetComponentInChildren<Spawner>();
            _tower = GetComponentInChildren<TowerController>();
        }

        public void Initialize()
        {
            _epochManager.TryGetCurrentEpoch(out _epoch);
            _tower.SetFaction(_faction);
            _spawner.SetFaction(_faction, _enemyFaction);
            _tower.SetData(_epoch.Tower);
        }

        public void StartGame()
        {
            _spawner.StartSpawn(_epoch);
        }
        
    }
}
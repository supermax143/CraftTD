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
        [SerializeField, HideInInspector] 
        private TowerController _tower;
        [SerializeField, HideInInspector] 
        private Spawner _spawner;
        
        
        [SerializeField] 
        private Faction _faction;
        [SerializeField] 
        private Faction _enemyFaction;
        
        [Inject] IMainModel _model;

        public EpochModel Epoch => _faction == Faction.Player ? _model.PlayerEpoch : _model.EnemyEpoch;
        
        
        
        public TowerController Tower => _tower;
        
        public Spawner Spawner => _spawner;

        public Faction Faction => _faction;


        private void OnValidate()
        {
            _spawner = GetComponentInChildren<Spawner>();
            _tower = GetComponentInChildren<TowerController>();
        }

        public void Initialize()
        {
            _tower.SetFaction(_faction);
            _spawner.SetFaction(_faction, _enemyFaction);
            _tower.SetData(Epoch.Tower.Entity);
        }

        public void StartGame()
        {
            _spawner.StartSpawn(Epoch);
        }
        
    }
}
using System;
using System.Linq;
using Core.Application.Models;
using Unity.Game.Attributes.Specific;
using Unity.Utils.Time;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace Unity.Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private float _spawnRange;
        [SerializeField] private Transform _spawnTransform;
       
        [Inject] private DiContainer _container;
        [Inject] private IGameController _gameController;
        
        private Faction _faction;
        private Faction _enemyFaction;

        protected EpochModel _epoch;
        private bool _canSpawn = false;

        private void Start()
        {
            _gameController.OnTowerDestroyed += TowerDestroyedHandler;
        }

        public void SetFaction(Faction faction, Faction enemyFaction)
        {
            _faction = faction;
            _enemyFaction = enemyFaction;
        }


        public virtual void StartSpawn(EpochModel epoch)
        {
            _canSpawn = true;
            _epoch = epoch;
        }

        protected virtual void TowerDestroyedHandler(Faction faction)
        {
            _canSpawn = false;
        }
        
        
        public void Spawn(UnitTier tier, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var spawnDelta = new Vector3(Random.Range(-_spawnRange, _spawnRange), 0, Random.Range(-_spawnRange, _spawnRange));

                var unitModel = _epoch.GetUnitByTier(tier);
                
                if (!unitModel.Info.TryGetAttribute<UnitPrefabAttribute>(out var unitPrefabAttribute))
                {
                    throw new System.Exception("No unit prefab found");
                }
                
                var prefab = unitPrefabAttribute.Value;//epoch.GetRandomUnitTier().TryGetAttribute();
                var unit = _container.InstantiatePrefabForComponent<UnitController>(prefab, _spawnTransform);
                unit.SetFaction(_faction, _enemyFaction);
                unit.transform.position = transform.position + spawnDelta;
                if(_gameController.TryGetOpponentTower(unit.OpponentFaction, out var target))
                {
                    unit.transform.LookAt(target.transform);
                }
                unit.SetData(unitModel.Info);
            }
        }

        private void OnDestroy()
        {
            _gameController.OnTowerDestroyed -= TowerDestroyedHandler;
        }
    }
}
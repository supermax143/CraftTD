using System;
using System.Linq;
using Unity.Game.Attributes.Specific;
using Unity.Utils.Time;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace Unity.Game
{
    public abstract class SpawnerBase : MonoBehaviour
    {
        [SerializeField] private float _spawnRange;
        [SerializeField] private Transform _spawnTransform;
       
        [Inject] private DiContainer _container;
        [Inject] private GameController _gameController;
        
        private Faction _faction;
        private Faction _enemyFaction;

        protected EpochData _epoch;
        /*private Timer _spawnTimer = new();
        private float _spawnDelay;*/

        
        /*private void Update()
        {
            _spawnTimer.Update();
        }*/

        private void Start()
        {
            _gameController.OnTowerDestroyed += TowerDestroyedHandler;
        }

        public void SetFaction(Faction faction, Faction enemyFaction)
        {
            _faction = faction;
            _enemyFaction = enemyFaction;
        }

        /*public void StartSpawn(float spawnDelay)
        {
            _gameController.OnTowerDestroyed += TowerDestroyedHandler;
            _spawnDelay = spawnDelay;
            _spawnTimer.Complete += OnSpawnTimerComplete;
            _spawnTimer.Start(_spawnDelay);
        }*/

        /*private void TowerDestroyedHandler(Faction obj)
        {
            _spawnTimer.Stop();
            _spawnTimer.Complete -= OnSpawnTimerComplete;
        }*/

        public virtual void StartSpawn(EpochData epoch)
        {
            _epoch = epoch;
        }
        protected abstract void TowerDestroyedHandler(Faction faction);
        
        /*private void OnSpawnTimerComplete()
        {
            Debug.Log("OnSpawnTimerComplete");
            Spawn();
            _spawnTimer.Start(_spawnDelay);
        }*/
        
        protected void Spawn(UnitTier tier, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var spawnDelta = new Vector3(Random.Range(-_spawnRange, _spawnRange), 0, Random.Range(-_spawnRange, _spawnRange));

                var unitData = _epoch.GetUnitDataByTier(tier);
                
                if (!unitData.TryGetAttribute<UnitPrefabAttribute>(out var unitPrefabAttribute))
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
                unit.SetData(unitData);
            }
        }

        private void OnDestroy()
        {
            _gameController.OnTowerDestroyed -= TowerDestroyedHandler;
        }
    }
}
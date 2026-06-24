using System;
using System.Collections;
using System.Linq;
using Core.Application.Models;
using Unity.Game.Attributes.Specific;
using Unity.Utils;
using Unity.Utils.Time;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace Unity.Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] 
        private float _spawnRange;
        [SerializeField]
        private Transform _spawnTransform;
        [SerializeField]
        private bool _blockSpawn = false;
        
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
            if (_blockSpawn)
            {
                return;
            }
            for (int i = 0; i < count; i++)
            {
                float angle = i * 2.39996f;
                float radius = Mathf.Sqrt(i) * (_spawnRange / Mathf.Sqrt(count));
                float x = radius * Mathf.Cos(angle);
                float y = radius * Mathf.Sin(angle);
                //добавить вариант для 3d
                
                var spawnDelta = new Vector3(x, y);

                if (count == 1)
                {
                    float randAng = Random.Range(0f, 360f);
                    spawnDelta = new Vector2(
                        Mathf.Cos(randAng * Mathf.Deg2Rad),
                        Mathf.Sin(randAng * Mathf.Deg2Rad)
                    );
                    spawnDelta *= .1f;
                }
                
                
                if (!_epoch.TryGetUnitModel(tier, out var unitModel))
                {
                    throw new System.Exception("No unit model found");
                }

                var prefab = unitModel.Info.UnitPrefab;
                var unit = _container.InstantiatePrefabForComponent<UnitController>(prefab, _spawnTransform);
                var layer = _faction == Faction.Player ? Layers.Player : Layers.Enemy;
                unit.gameObject.SetLayerRecursively(layer);
                unit.SetFaction(_faction, _enemyFaction);
                unit.transform.position = transform.position + spawnDelta;
                unit.SetData(unitModel.Entity);
                StartCoroutine(RandomizeAnimation(unit.View));
            }
        }

        private IEnumerator RandomizeAnimation(UnitView unit)
        {
            yield return new WaitForSeconds(.3f);
            unit.SetRandomFrame();
        }
        
        
        private void OnDestroy()
        {
            _gameController.OnTowerDestroyed -= TowerDestroyedHandler;
        }
    }
}
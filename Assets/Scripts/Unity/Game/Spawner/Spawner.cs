using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Unity.Infrastructure.ResourceManager;
using Unity.Utils;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Unity.Game
{
    public class Spawner : MonoBehaviour
    {
        public event Action<UnitController> OnUnitSpawned;
        
        
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
            _gameController.OnGameFinished += TowerDestroyedHandler;
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
        
        
        public async Task Spawn(UnitTier tier, int count)
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
                var assetReference = unitModel.Info.UnitPrefab;
                var prefab = await assetReference.LoadAssetReference<GameObject>(assetReference.AssetGUID);
                var unit = _container.InstantiatePrefabForComponent<UnitController>(prefab, _spawnTransform);
                var layer = _faction == Faction.Player ? Layers.Player : Layers.Enemy;
                unit.gameObject.SetLayerRecursively(layer);
                unit.SetFaction(_faction, _enemyFaction);
                unit.transform.position = transform.position + spawnDelta;
                unit.SetData(unitModel.Entity);
                OnUnitSpawned?.Invoke(unit);
                StartCoroutine(RandomizeAnimation(unit.View));
            }
        }

        private IEnumerator RandomizeAnimation(UnitView unit)
        {
            if (unit == null)
            {
                yield break;
            }
            yield return new WaitForSeconds(.3f);
            unit.SetRandomFrame();
        }

        public virtual void Reset()
        {
            _canSpawn = false;
            _epoch = null;
        }
        
        private void OnDestroy()
        {
            _gameController.OnGameFinished -= TowerDestroyedHandler;
        }
    }
}
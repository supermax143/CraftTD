using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Unity.Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _unitPrefabs;
        [SerializeField] private float _spawnRange;
        [SerializeField] private Transform _spawnTransform;
       
        [Inject] private DiContainer _container;
        [Inject] private GameController _gameController;
      
        public UnitController Spawn(Faction faction, Faction enemyFaction)
        {
            var spawnDelta = new Vector3(Random.Range(-_spawnRange, _spawnRange), 0, Random.Range(-_spawnRange, _spawnRange));
            
            var prefab = _unitPrefabs[Random.Range(0, _unitPrefabs.Length)];
            var unit = _container.InstantiatePrefabForComponent<UnitController>(prefab, _spawnTransform);
            unit.SetFaction(faction, enemyFaction);
            unit.transform.position = transform.position + spawnDelta;
            if(_gameController.TryGetOpponentTower(unit.OpponentFaction, out var target))
            {
                unit.transform.LookAt(target.transform);
            }
            unit.Initialize();
            return  unit;
        }

        
    }
}
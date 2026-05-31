using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Unity.Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject _unitPrefab;
        [SerializeField] private float _spawnRange;
        [SerializeField] private Transform _spawnTransform;
       
        [Inject] private DiContainer _container;
        
      
        public UnitController Spawn(Faction faction, Faction enemyFaction)
        {
            var spawnDelta = new Vector3(Random.Range(-_spawnRange, _spawnRange), 0, Random.Range(-_spawnRange, _spawnRange));
            
            var unit = _container.InstantiatePrefabForComponent<UnitController>(_unitPrefab, _spawnTransform);
            // var unit = Instantiate(_unitPrefab, _spawnTransform).GetComponent<UnitController>();
            unit.SetFaction(faction, enemyFaction);
            unit.transform.position = transform.position + spawnDelta;
            return  unit;
        }

        
    }
}
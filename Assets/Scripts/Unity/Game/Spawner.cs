using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Game
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject _unitPrefab;
        [SerializeField] private float _spawnRange;
        [SerializeField] private Transform _spawnTransform;
       
      
        public UnitActor Spawn()
        {
            var spawnDelta = new Vector3(Random.Range(-_spawnRange, _spawnRange), 0, Random.Range(-_spawnRange, _spawnRange));
            var unit = Instantiate(_unitPrefab, _spawnTransform).GetComponent<UnitActor>();
            unit.transform.position = transform.position + spawnDelta;
            return  unit;
        }
        
    }
}
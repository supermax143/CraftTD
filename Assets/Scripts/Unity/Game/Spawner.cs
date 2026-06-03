using System.Linq;
using Unity.Game.Attributes.Specific;
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
        [Inject] private ChronologyData _chronologyData;
      
        public UnitController Spawn(Faction faction, Faction enemyFaction)
        {
            var epoch = _chronologyData.Epochs.First();
            
            var spawnDelta = new Vector3(Random.Range(-_spawnRange, _spawnRange), 0, Random.Range(-_spawnRange, _spawnRange));

            var unitData = epoch.GetRandomUnitTier();
            
            if (!unitData.TryGetAttribute<UnitPrefabAttribute>(out var unitPrefabAttribute))
            {
                throw new System.Exception("No unit prefab found");
            }
            
            var prefab = unitPrefabAttribute.Value;//epoch.GetRandomUnitTier().TryGetAttribute();
            var unit = _container.InstantiatePrefabForComponent<UnitController>(prefab, _spawnTransform);
            unit.SetFaction(faction, enemyFaction);
            unit.transform.position = transform.position + spawnDelta;
            if(_gameController.TryGetOpponentTower(unit.OpponentFaction, out var target))
            {
                unit.transform.LookAt(target.transform);
            }
            unit.SetData(unitData);
            return  unit;
        }

        
    }
}
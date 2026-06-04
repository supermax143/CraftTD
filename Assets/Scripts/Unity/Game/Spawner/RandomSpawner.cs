using System.Collections;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game
{
    public class RandomSpawner : SpawnerBase
    {
        [SerializeField]
        private float _spawnDelay;
        [SerializeField]
        private int _count = 1;
        
        
        public override void StartSpawn(EpochData epoch)
        {
            base.StartSpawn(epoch);
            StartCoroutine(WaitSpawnDelay());
        }

        private IEnumerator WaitSpawnDelay()
        {
            yield return  new WaitForSeconds(_spawnDelay);
            var tier = _epoch.GetRandomUnitTier();
            Spawn(tier, _count);
            StartCoroutine(WaitSpawnDelay());
        }
        
        protected override void TowerDestroyedHandler(Faction faction)
        {
            StopAllCoroutines();
        }
        
        
    }
}
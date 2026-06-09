using System.Collections;
using UnityEngine;

namespace Unity.Game
{
    public class WavesSpawner : Spawner
    {
        private int _waveIndex;
        
        public override void StartSpawn(EpochInfo epoch)
        {
            base.StartSpawn(epoch);
            StartCoroutine(SpawnNextWave());
        }

        private IEnumerator SpawnNextWave()
        {
            if (_waveIndex >= _epoch.Waves.Count)
            {
                Debug.Log("All waves spawned");
                yield break;
            }
            
            var wave = _epoch.Waves[_waveIndex];
            foreach (var squad in wave.Squads)
            {
                yield return new WaitForSeconds(squad.Delay);
                Spawn(squad.Tier, squad.Count);
            }
            _waveIndex++;
            
            StartCoroutine(SpawnNextWave());
        }
        
        
        protected override void TowerDestroyedHandler(Faction faction)
        {
            base.TowerDestroyedHandler(faction);
            StopAllCoroutines();
        }
    }
}
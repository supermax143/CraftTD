using System.Collections;
using Core.Application.Models;
using UnityEngine;

namespace Unity.Game
{
    public class WavesSpawner : Spawner
    {
        private int _waveIndex;
        
        public override void StartSpawn(EpochModel epoch)
        {
            base.StartSpawn(epoch);
            StartCoroutine(SpawnNextWave());
        }

        private IEnumerator SpawnNextWave()
        {
            if (_waveIndex >= _epoch.Info.Waves.Count)
            {
                Debug.Log("All waves spawned");
                yield break;
            }
            
            var wave = _epoch.Info.Waves[_waveIndex];
            foreach (var squad in wave.Squads)
            {
                yield return new WaitForSeconds(squad.Delay);
                Spawn(squad.Tier, squad.Count);
                yield break;
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
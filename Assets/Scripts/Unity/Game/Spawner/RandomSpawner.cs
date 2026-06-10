using System.Collections;
using Core.Application.Models;
using Unity.Utils.Time;
using UnityEngine;

namespace Unity.Game
{
    public class RandomSpawner : Spawner
    {
        [SerializeField]
        private float _spawnDelay;
        [SerializeField]
        private int _count = 1;
        
        
        public override void StartSpawn(EpochModel epoch)
        {
            base.StartSpawn(epoch);
            StartCoroutine(WaitSpawnDelay());
        }

        private IEnumerator WaitSpawnDelay()
        {
            yield return  new WaitForSeconds(_spawnDelay);
            var tier = _epoch.Info.GetRandomUnitTier();
            Spawn(tier, _count);
            StartCoroutine(WaitSpawnDelay());
        }
        
        protected override void TowerDestroyedHandler(Faction faction)
        {
            base.TowerDestroyedHandler(faction);
            StopAllCoroutines();
        }
        
        
    }
}
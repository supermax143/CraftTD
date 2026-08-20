using System.Collections;
using Core.Application.Models;
using Unity.Infrastructure.Effects;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class WavesSpawner : Spawner
    {
        private int _waveIndex;
        private Coroutine _spawnCorotine;
        
        [Inject] private PopupSpawnManager _popups;
        
        public override void StartSpawn(EpochModel epoch)
        {
            base.StartSpawn(epoch);
           _spawnCorotine = StartCoroutine(SpawnNextWave());
        }

        private bool IsLastWave(int waveIndex)
        {
            return waveIndex >= _epoch.Info.Waves.Count;
        }
        
        private IEnumerator SpawnNextWave()
        {
            bool showPopup = true;
            if (IsLastWave(_waveIndex))
            {
                Debug.Log("All waves spawned");
                yield break;
            }
            
            var wave = _epoch.Info.Waves[_waveIndex];
            foreach (var squad in wave.Squads)
            {
                yield return new WaitForSeconds(_gameStats.GetSquadDelay(squad.Delay));
                if (showPopup)
                {
                    ShowWavePopup();
                    showPopup = false;
                }
                Spawn(squad.Tier, squad.Count);
            }
            _waveIndex++;
            _spawnCorotine = StartCoroutine(SpawnNextWave());
        }

        private void ShowWavePopup()
        {
            var waveText = $"Волна {_waveIndex + 1}";
            if (IsLastWave(_waveIndex + 1))
            {
                waveText = "Последняя волна";
            }
            _popups.SpawnTextPopup(waveText, PopupType.TextNextWave);
        }


        protected override void TowerDestroyedHandler(Faction faction)
        {
            base.TowerDestroyedHandler(faction);
            StopAllCoroutines();
        }

        public override void Reset()
        {
            if (_spawnCorotine != null)
            {
                StopCoroutine(_spawnCorotine);
            }
            _waveIndex = 0;
            base.Reset();
        }
    }
}
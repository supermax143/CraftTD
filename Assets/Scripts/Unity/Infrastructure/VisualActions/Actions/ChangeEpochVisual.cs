using System.Collections;
using Unity.Game;
using Unity.Infrastructure.VisualActions.ActionsData;
using Unity.Settings;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.VisualActions.Actions
{
    public class ChangeEpochVisual : VisualActionBase<ChangeEpochActionData>
    {

        [Inject] private IGameController _gameController;
        [Inject] private GameSettings _gameSettings;
        
        private Coroutine _animationCoroutine;
        
        public override void Execute()
        {
            StartCoroutine(ShowEpochChanging(Data.ShowInversed));
        }
        
        private IEnumerator ShowEpochChanging(bool inversed)
        {
            var time = _gameSettings.EpochChangeTime; 
            _gameController.BlocUI();
            var enemyTeam = _gameController.GetTeam(Faction.Enemy);
            enemyTeam.InstantiateAndShowNextTower(Data.EnemyEpoch.Tower.Info.TowerPrefab, time, !inversed);
            enemyTeam.Tower.Hide(time, inversed);
            _gameController.LocationContainer.SwitchToLocation(Data.EnemyEpoch.Info.LocationPrefab, time, inversed);
            yield return new WaitForSeconds(time);
            _gameController.Reset();
            _gameController.UnblockUI();
            Complete();
        }
        
    }
}
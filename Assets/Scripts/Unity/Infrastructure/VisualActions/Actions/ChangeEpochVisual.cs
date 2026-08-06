using System.Collections;
using Core.Application.Models;
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
        [Inject] private IMainModel _model;
        
        private EpochModel PlayerEpoch => _model.PlayerEpoch;
        private EpochModel EnemyEpoch => _model.EnemyEpoch;
        
        private Coroutine _animationCoroutine;
        
        public override void Execute()
        {
            StartCoroutine(ShowEpochChanging(Data.ShowInversed));
        }
        
        private IEnumerator ShowEpochChanging(bool inversed)
        {
            var time = _gameSettings.EpochChangeTime; 
            _gameController.BlocUI();
            UpdateTeamTower( Faction.Enemy, inversed, time);
            if (Data.ChangePlayerTower)
            {
                UpdateTeamTower(Faction.Player,inversed, time);
            }
            _gameController.LocationContainer.SwitchToLocation(EnemyEpoch.Info.LocationPrefab, time, inversed);
            yield return new WaitForSeconds(time);
            _gameController.Reset();
            _gameController.UnblockUI();
            Complete();
        }

        private void UpdateTeamTower(Faction faction,bool inversed, float time)
        {
            var team = _gameController.GetTeam(faction);
            var epoch = faction == Faction.Player ? PlayerEpoch : EnemyEpoch;
            team.InstantiateAndShowNewTower(epoch.Tower.Info.TowerPrefab, time, !inversed);
            if (!team.TowerDestroyed)
            {
                team.Tower.Hide(time, inversed);
            }
        }
    }
}
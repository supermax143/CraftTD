using System.Collections;
using Core.Application.Interfaces.Windows;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Game;
using Unity.Infrastructure.Advertisement;
using Unity.Infrastructure.Advertisement.Transactions;
using Unity.Infrastructure.VisualActions.ActionsData;
using Unity.Presentation;
using Unity.Presentation.Windows.Result;
using Unity.Settings;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.VisualActions.Actions
{
    public class ShowResultVisual : VisualActionBase<ShowResultActionData>
    {
        [Inject] private IGameController _gameController;
        [Inject] private ILevelRewardAggregator _rewardAggregator;
        [Inject] private IWindowsController _windowsController;
        [Inject] private IAdvertisementController _advertisementController;
        [Inject] private IMainModel _mainModel;
        [Inject] private BattleView _hud;
        [Inject] private GameSettings _gameSettings;
        
        private EpochModel PlayerEpoch => _mainModel.PlayerEpoch;
        private EpochModel EnemyEpoch => _mainModel.EnemyEpoch;
        
        
        private ResultWindow _resultWindow;


        public override void Execute()
        {
            ShowResultWindow(Data.PlayerWin);
        }
        
        public async UniTask ShowResultWindow(bool playerWin)
        {
            _resultWindow = await _windowsController.ShowWindow<ResultWindow>();
            _resultWindow.SetResult(_rewardAggregator.Money, playerWin);
            _resultWindow.Show();
            _resultWindow.OnAdStartWatch += WatchAdForDoubleMoney;
            _resultWindow.OnHide += OnResultWindowClose;
        }
        
        private void WatchAdForDoubleMoney()
        {
            _advertisementController.AddListener<AdvertisementDoubleReward>(OnAdWatched);
            _advertisementController.ShowRewardedDoubleReward();
        }
        
        private void OnAdWatched(AdvertisementDoubleReward adv)
        {
            if (adv.AdvResult == AdvertisementBase.Result.Completed)
            {
                _rewardAggregator.AddMoney(_rewardAggregator.Money.Value);
            }
            _resultWindow.UpdateReward(_rewardAggregator.Money);
            DOVirtual.DelayedCall(1.5f, () => _resultWindow.Hide());
        }

        private void OnResultWindowClose(IWindow iWindow)
        {
            _resultWindow.OnAdStartWatch -= WatchAdForDoubleMoney;
            _resultWindow.OnHide -= OnResultWindowClose;
            _resultWindow = null;
            _mainModel.Inventory.Money += _rewardAggregator.Money;
            _rewardAggregator.Reset();
            _hud.SetIsBattleState(false);
            StartCoroutine(ResetLevel());
            //_gameController.Reset();
        }

        private IEnumerator ResetLevel()
        {

            if (Data.ResetTower)
            {
                var faction = Data.PlayerWin ? Faction.Enemy : Faction.Player;
                UpdateTeamTower(faction, faction == Faction.Enemy, _gameSettings.EpochChangeTime);
                _gameController.GetTeam(Faction.Player).HideUnits(_gameSettings.EpochChangeTime, false);
                _gameController.GetTeam(Faction.Enemy).HideUnits(_gameSettings.EpochChangeTime, false);
                yield return new WaitForSeconds(_gameSettings.EpochChangeTime);
                _gameController.Reset();
            }
            
            Complete();
        }
        
        
        private void UpdateTeamTower(Faction faction,bool inversed, float time)
        {
            var team = _gameController.GetTeam(faction);
            var epoch = faction == Faction.Player ? PlayerEpoch : EnemyEpoch;
            team.InstantiateAndShowNewTower(epoch.Tower.Info.TowerPrefab, time, !inversed);
            team.HideTower(time, inversed);
            /*if (!team.TowerDestroyed)
            {
                team.Tower.Hide(time, inversed);
            }*/
        }
        
    }
}

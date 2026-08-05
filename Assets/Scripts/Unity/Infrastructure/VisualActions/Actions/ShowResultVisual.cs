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
        [Inject] private HUDGameView _hud;
        
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
            _gameController.Reset();
            Complete();
        }
        
    }
}

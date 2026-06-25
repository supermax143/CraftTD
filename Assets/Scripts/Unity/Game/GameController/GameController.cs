using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Application.Interfaces.ApplicationSession;
using Core.Application.Interfaces.Windows;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Unity.Infrastructure.Advertisement;
using Unity.Infrastructure.Advertisement.Transactions;
using Unity.Presentation.Windows.Result;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class GameController : MonoBehaviour, IGameController
    {
        
        public event Action<Faction> OnTowerDestroyed;
        
        [SerializeField]
        private List<Team> _teams;
        [SerializeField] 
        private FoodProduction _foodProduction;

        [Inject] private IApplicationSession _applicationSession;
        [Inject] private IMainModel _mainModel;
        [Inject] private ILevelRewardAggregator _rewardAggregator;
        [Inject] private IWindowsController _windowsController;
        [Inject] private IAdvertisementController _advertisementController;
        
        private EpochModel Epoch => _mainModel.PlayerEpoch;
        
        private Spawner _spawner;
        private bool _started = false;

        private ResultWindow _resultWindow;
        
        
        private void Start()
        {
            foreach (var team in _teams)
            {
                team.Initialize();
                team.Tower.OnDestroyed += TowerDestroyedHandler;
                if (team.Faction == Faction.Player)
                {
                    _spawner = team.Spawner;
                }
            }
        }

        public void StartGame()
        {
            if (_started)
            {
                return;
            }
            
            foreach (var team in _teams)
            {
                team.StartGame();
            }
            _foodProduction.StartProduction();
            _started = true;
        }

        private void TowerDestroyedHandler(TowerController tower)
        {
            _foodProduction.StopProduction();
            OnTowerDestroyed?.Invoke(tower.Faction);
            ShowResultDelayed(tower.Faction != Faction.Player);
        }

        public async UniTask ShowResultDelayed(bool isVictory)
        {
            UniTask.WaitForSeconds(1);
            _resultWindow = await _windowsController.ShowWindow<ResultWindow>();
            _resultWindow.SetResult(_rewardAggregator.Money, isVictory);
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
            _mainModel.PlayerEpoch.Money += Resource.Money(_rewardAggregator.Money.Value);
            _started = false;
            _foodProduction.Reset();
            foreach (var team in _teams)
            {
                team.Reset();
            }
        }


        public bool TryGetOpponentTower(Faction opponentFaction,out AttackTargetBase target)
        {
            target = default;
            var team = _teams.FirstOrDefault(team => team.Tower.Faction == opponentFaction);
            if (team == null)
            {
                return false;
            }
            
            target = team.Tower.AttackTarget;
            return true;
        }
        
        public void BuyUnit(UnitTier tier)
        {
            
            if (!Epoch.TryGetUnitModel(tier, out var unit)||
                _foodProduction.FoodCount < unit.FoodCost)
            {
                return;
            }
            _foodProduction.WithdrawFood(unit.FoodCost);
            _spawner.Spawn(tier, 1);
        }

        public void ExitGame()
        {
            _applicationSession.CurrentState.ExitGame();
        }
    }
}
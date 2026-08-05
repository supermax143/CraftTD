using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.Interfaces.ApplicationSession;
using Core.Application.Interfaces.Windows;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Unity.Infrastructure.Advertisement;
using Unity.Infrastructure.Advertisement.Transactions;
using Unity.Infrastructure.VisualActions;
using Unity.Infrastructure.VisualActions.ActionsData;
using Unity.Presentation;
using Unity.Presentation.Windows.Result;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

namespace Unity.Game
{
    public class GameController : MonoBehaviour, IGameController
    {
        
        public event Action<Faction> OnGameFinished;
        
        [SerializeField]
        private List<Team> _teams;
        [SerializeField] 
        private FoodProduction _foodProduction;
        [SerializeField]
        private HUDGameView _hud;
        /*[SerializeField]
        private Transform _locationPlaceholder;*/
        [SerializeField]
        private LocationContainer _locationContainer;
        [SerializeField]
        private float _epochSwitchingTime = 1f;
        
        [Inject] private IApplicationSession _applicationSession;
        [Inject] private IMainModel _mainModel;
        [Inject] private ILevelRewardAggregator _rewardAggregator;
        [Inject] private IWindowsController _windowsController;
        [Inject] private IAdvertisementController _advertisementController;
        [Inject] private IVisualActionsController _visualActionsController;
        
        private EpochModel Epoch => _mainModel.PlayerEpoch;
        private EpochModel EnemyEpoch => _mainModel.EnemyEpoch;
        public LocationContainer LocationContainer => _locationContainer;

        private Spawner _spawner;
        private bool _started = false;
        private bool _isPaused = false;
        private bool _uiBlocked = false;
        
        private ResultWindow _resultWindow;
        
        
        private void Start()
        {
            foreach (var team in _teams)
            {
                team.UpdateView();
                team.OnTowerDestroyed += TowerDestroyedHandler;
                if (team.Faction == Faction.Player)
                {
                    _spawner = team.Spawner;
                }
            }
            _locationContainer.Initialize(EnemyEpoch.Info.LocationPrefab);
            //UpdateLocation();
            //_mainModel.OnEnemyEpochChanged += UpdateView;
            //_mainModel.OnPlayerEpochChanged += UpdateView;
        }

        public void BlocUI() => _uiBlocked = true;
        public void UnblockUI() => _uiBlocked = false;
        
        public void StartGame()
        {
            if (_started)
            {
                return;
            }
            
            _hud.SetIsUpgradeState(false);
            foreach (var team in _teams)
            {
                team.StartGame();
            }
            _foodProduction.StartProduction();
            _started = true;
        }
        
        public void EndGame(Faction winner)
        {
            _foodProduction.StopProduction();
            OnGameFinished?.Invoke(winner);
            var playerWin = winner == Faction.Player;
            if (winner == Faction.Player &&
                _mainModel.CurrentEnemyEpochNumber <= _mainModel.CurrentPlayerEpochNumber)
            {
                _mainModel.IncreaseEnemyEpoch();
            }
            ShowResultDelayed(playerWin);
        }

        public Team GetTeam(Faction faction)
        {
            return _teams.FirstOrDefault(team => team.Faction == faction);
        }
        
        private void TowerDestroyedHandler(TowerController tower)
        {
            var winner = tower.Faction == Faction.Player ? Faction.Enemy : Faction.Player;
            EndGame(winner);
        }

        private async UniTask ShowResultDelayed(bool playerWin)
        {
            await UniTask.WaitForSeconds(1);
            await ShowResultWindow(playerWin);
        }

        private void Update()
        {

#if DEBUG_MODE
            UpdateEditorShortcuts();
#endif
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
            _started = false;
            _hud.SetIsUpgradeState(true);
            Reset();
        }

        public void Reset()
        {
            _foodProduction.Reset();
            foreach (var team in _teams)
            {
                team.Reset();
            }
            
        }
        
        public void SelectNextEnemyEpoch()
        {
            if (_uiBlocked)
            {
                return;
            }
            _mainModel.SelectEnemyEpochIndex(_mainModel.SelectedEnemyEpochIndex + 1);
            //StartCoroutine(ShowEpochSwitching(false));
            _visualActionsController.AddAction(new ChangeEpochActionData(false, EnemyEpoch));
        }

        public void SelectPrevEnemyEpoch()
        {
            if (_uiBlocked)
            {
                return;
            }
            _mainModel.SelectEnemyEpochIndex(_mainModel.SelectedEnemyEpochIndex - 1);
            _visualActionsController.AddAction(new ChangeEpochActionData(true, EnemyEpoch));
            //StartCoroutine(ShowEpochSwitching(true));
        }

        /*private IEnumerator ShowEpochSwitching(bool inversed)
        {
            _uiBlocked = true;
            var enemyTeam = GetTeam(Faction.Enemy);
            enemyTeam.InstantiateAndShowNextTower(EnemyEpoch.Tower.Info.TowerPrefab, _epochSwitchingTime, !inversed);
            enemyTeam.Tower.Hide(_epochSwitchingTime, inversed);
            _locationContainer.SwitchToLocation(EnemyEpoch.Info.LocationPrefab, _epochSwitchingTime, inversed);
            yield return new WaitForSeconds(_epochSwitchingTime);
            UpdateView();
            _uiBlocked = false;
        }*/
        
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

        public void Pause(bool pause)
        {
            _isPaused = pause;
            Time.timeScale = pause ? 0f : 1f;
        }

        public async Task ShowResultWindow(bool playerWin)
        {
            if (_resultWindow != null)
            {
                return;
            }
            _resultWindow = await _windowsController.ShowWindow<ResultWindow>();
            _resultWindow.SetResult(_rewardAggregator.Money, playerWin);
            _resultWindow.Show();
            _resultWindow.OnAdStartWatch += WatchAdForDoubleMoney;
            _resultWindow.OnHide += OnResultWindowClose;
        }

        public void ExitGame()
        {
            _applicationSession.CurrentState.ExitGame();
        }

        /*private void OnDestroy()
        {
            _mainModel.OnEnemyEpochChanged -= UpdateView;
            _mainModel.OnPlayerEpochChanged -= UpdateView;
        }*/

#if DEBUG_MODE
        private void UpdateEditorShortcuts()
        {
            
            var keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard.tKey.wasPressedThisFrame)
            {
                Time.timeScale = 10f;
            }

            if (keyboard.tKey.wasReleasedThisFrame)
            {
                Time.timeScale = 1f;
            }
           
        }
#endif
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.Interfaces.ApplicationSession;
using Core.Application.Interfaces.Views;
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
using Unity.Presentation.HUD;
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
        private GameHUD _hud;
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
        [Inject] private IActionsDispatcher _actionsDispatcher;
        [Inject] private IViewsController _viewsController;
        [Inject] private DropManager _dropManager;
        
        private EpochModel PlayerEpoch => _mainModel.PlayerEpoch;
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
        }

        public void BlocUI() => _uiBlocked = true;
        public void UnblockUI() => _uiBlocked = false;
        
        public void StartBattle()
        {
            if (_started)
            {
                return;
            }
            
            _hud.ShowBattleView();
            if(!_viewsController.TryGetCurrentView(out var view) || !(view is BattleView battleView))
            {
                Debug.Log("current view is not battle");
                return;
            }
            
            battleView.SetIsBattleState(true);
            foreach (var team in _teams)
            {
                team.StartGame();
            }
            _foodProduction.StartProduction();
            _started = true;
        }
        
        public void FinishRound(Faction winner, bool force)
        {
            var playerWin = winner == Faction.Player;
            var epochIncreased = playerWin &&
                                 _mainModel.CurrentEnemyEpochNumber <= _mainModel.CurrentPlayerEpochNumber &&
                                 _mainModel.CurrentEnemyEpochNumber - 1 == _mainModel.SelectedEnemyEpochIndex;
            var resetTower = !epochIncreased ||
                             (playerWin && _mainModel.CurrentEnemyEpochNumber == _mainModel.CurrentPlayerEpochNumber);
            
            _foodProduction.StopProduction();
            OnGameFinished?.Invoke(winner);
            _actionsDispatcher.AddAction(new ShowResultActionData(playerWin, resetTower, force, 1.5f));
            if (epochIncreased)
            {
                _mainModel.IncreaseEnemyEpoch();
                if (_mainModel.CurrentEnemyEpochNumber <= _mainModel.CurrentPlayerEpochNumber)
                {
                    _actionsDispatcher.AddAction(new ChangeEpochActionData(false, false));
                }
            }
        }

        public Team GetTeam(Faction faction)
        {
            return _teams.FirstOrDefault(team => team.Faction == faction);
        }
        
        private void TowerDestroyedHandler(TowerController tower)
        {
            var winner = tower.Faction == Faction.Player ? Faction.Enemy : Faction.Player;
            FinishRound(winner, false);
        }


        private void Update()
        {

#if DEBUG_MODE
            UpdateEditorShortcuts();
#endif
        }
        
        public void Reset()
        {
            _started = false;
            _foodProduction.Reset();
            _dropManager.Reset();
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
            _actionsDispatcher.AddAction(new ChangeEpochActionData(false, false));
        }

        public void SelectPrevEnemyEpoch()
        {
            if (_uiBlocked)
            {
                return;
            }
            _mainModel.SelectEnemyEpochIndex(_mainModel.SelectedEnemyEpochIndex - 1);
            _actionsDispatcher.AddAction(new ChangeEpochActionData(true, false));
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
            
            if (!PlayerEpoch.TryGetUnitModel(tier, out var unit)||
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

#if DEBUG_MODE
        public void ExitGame()
        {
            _applicationSession.CurrentState.ExitGame();
        }


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
        private void OnDestroy()
        {
            foreach (var team in _teams)
            {
                team.OnTowerDestroyed -= TowerDestroyedHandler;
            }
        }
    }
}
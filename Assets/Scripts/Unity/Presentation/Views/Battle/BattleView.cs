using System;
using System.Threading.Tasks;
using Core.Application.Interfaces.Windows;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Environments.Land.Scripts.Runtime.GUI;
using TMPro;
using Unity.Game;
using Unity.Presentation.Components;
using Unity.Presentation.HUD;
using Unity.Presentation.HUD.SpellsPanel;
using Unity.Presentation.Views;
using Unity.Presentation.Windows;
using Unity.Presentation.Windows.Pause;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Unity.Presentation
{
    public class BattleView : ViewBase
    {
        [SerializeField, HideInInspector]
        private BattleViewAnimatorController _animator;
        [SerializeField, HideInInspector]
        private StartBattlePanel _startBattlePanel;
        [SerializeField]
        private UnitsBuyPanel _unitsBuyPanel;
        [SerializeField]
        private SpellsCastPanel _spellsPanel;
        
        [Inject] private ILevelRewardAggregator _rewardAggregator;
        [Inject] private IWindowsController _windowsController;
        [Inject] private IGameController _gameController;


        protected override void OnValidate()
        {
            base.OnValidate();
            _animator = GetComponentInChildren<BattleViewAnimatorController>();
            _startBattlePanel = GetComponentInChildren<StartBattlePanel>();
            _unitsBuyPanel = GetComponentInChildren<UnitsBuyPanel>();
        }


        public void ShowUpgradeWindow()
        {
           /*_windowsController.ShowWindow<UpgradeWindow>(window =>
           {
               window.Show();
           });*/
        }
        
        public void ShowPauseWindow()
        {
            _windowsController.ShowWindow<PauseWindow>(window =>
            {
                window.Initialize(_gameController);
                window.Show();
            });
        }
        
        public void SetIsBattleState(bool value)
        {
            _animator.SetIsBattleState(value);
            if (!value)
            {
                _unitsBuyPanel.Clear();
                _spellsPanel.Dispose();
            }
            else
            {
                _spellsPanel.UpdateView();
                _unitsBuyPanel.UpdateView().Forget();
            }
        }
    }
}
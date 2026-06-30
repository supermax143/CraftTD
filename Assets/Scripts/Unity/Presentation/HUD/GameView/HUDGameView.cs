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
using Unity.Presentation.Windows;
using Unity.Presentation.Windows.Pause;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace Unity.Presentation
{
    public class HUDGameView : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private HUDGameAnimatorController _animator;
        [SerializeField, HideInInspector]
        private StartBattlePanel _startBattlePanel;
        [SerializeField]
        private UnitsBuyPanel _unitsBuyPanel;
        [SerializeField]
        private ResourceContainer _earnedMoney;
        
        [Inject] private ILevelRewardAggregator _rewardAggregator;
        [Inject] private IWindowsController _windowsController;
        [Inject] private IGameController _gameController;

        private void OnValidate()
        {
            _animator = GetComponentInChildren<HUDGameAnimatorController>();
            _startBattlePanel = GetComponentInChildren<StartBattlePanel>();
            _unitsBuyPanel = GetComponentInChildren<UnitsBuyPanel>();
        }

        public void UpdateView()
        {
            _startBattlePanel.UpdateView();
            _unitsBuyPanel.UpdateView();
            _earnedMoney.SetValue(0);
        }
        
      
        public void ShowUpgradeWindow()
        {
           _windowsController.ShowWindow<UpgradeWindow>(window =>
           {
               window.Show();
           });
        }
        
        public void ShowPauseWindow()
        {
            _windowsController.ShowWindow<PauseWindow>(window =>
            {
                window.Initialize(_gameController);
                window.Show();
            });
        }
        
        public void SetIsUpgradeState(bool value)
        {
            _animator.SetIsUpgradeState(value);
            if (value)
            {
                _unitsBuyPanel.Clear();
            }
            else
            {
                _unitsBuyPanel.UpdateView();
            }
        }
    }
}
using System;
using Core.Application.Interfaces.Windows;
using DG.Tweening;
using Environments.Land.Scripts.Runtime.GUI;
using TMPro;
using Unity.Game;
using Unity.Presentation.Components;
using Unity.Presentation.Windows;
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
        
        
        [Inject] private ILevelRewardAggregator _rewardAggregator;
        [Inject] private IWindowsController _windowsController;

        private void OnValidate()
        {
            _animator = GetComponentInChildren<HUDGameAnimatorController>();
            _startBattlePanel = GetComponentInChildren<StartBattlePanel>();
        }

        private void Start()
        {
            _startBattlePanel.UpdateView();
        }
        
        public void ShowExampleWindow()
        {
           _windowsController.ShowWindow<UpgradeWindow>(window =>
           {
               window.Show();
           });
        }
        
        public void SetIsUpgradeState(bool value)
        {
            _animator.SetIsUpgradeState(value);
        }
    }
}
using System;
using Core.Application.Interfaces.Windows;
using TMPro;
using Unity.Game;
using Unity.Presentation.Components;
using Unity.Presentation.Windows;
using UnityEngine;
using Zenject;

namespace Unity.Presentation
{
    public class HUDGameView : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private HUDGameAnimatorController _animator;
        
        
        [Inject] private ILevelRewardAggregator _rewardAggregator;
        [Inject] private IWindowsController _windowsController;

        private void OnValidate()
        {
            _animator = GetComponent<HUDGameAnimatorController>();
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
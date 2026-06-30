using System;
using Core.Application.Models;
using TMPro;
using Unity.Game;
using Unity.Infrastructure.Advertisement;
using Unity.Infrastructure.Advertisement.Transactions;
using Unity.Infrastructure.Windows;
using Unity.Presentation.HUD;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows.Result
{
    [Window(nameof(ResultWindow))]
    public class ResultWindow : WindowBase
    {
        public event Action OnAdStartWatch;
        
        [SerializeField]
        private TextMeshProUGUI _resultLabel;
        [SerializeField]
        private ResourceContainer _resourceContainer;

        
        private bool _isVictory;

        public void SetResult(Resource reward, bool isVictory)
        {
            UpdateReward(reward);
            _isVictory = isVictory;
            UpdateView();
        }
        
        public void UpdateReward(Resource reward)
        {
            _resourceContainer.SetValue(reward.Value);
        }
        
        private void UpdateView()
        {
            _resultLabel.text = _isVictory ? "Victory" : "Defeat";
        }


        public void WatchAdForDoubleMoney()
        {
            OnAdStartWatch?.Invoke();
        }

        
    }
}

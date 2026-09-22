using System;
using Core.Application.Interfaces;
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

        [Inject] private ILocalization _localization;
        
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
            var locale = _isVictory ? "result_window_label_win" : "result_window_label_lose";
            _resultLabel.text = _localization.Get(locale);
        }


        public void WatchAdForDoubleMoney()
        {
            OnAdStartWatch?.Invoke();
        }

        public Vector2 GetRewardPosition()
        {
            return _resourceContainer.transform.position;
        }
    }
}

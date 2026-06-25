using System;
using Core.Application.Models;
using TMPro;
using Unity.Game;
using Unity.Infrastructure.Advertisement;
using Unity.Infrastructure.Advertisement.Transactions;
using Unity.Infrastructure.Windows;
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
        private TextMeshProUGUI _moneyLabel;

        
        private bool _isVictory;
        private Resource _reward;

        public void SetResult(Resource reward, bool isVictory)
        {
            UpdateReward(reward);
            _isVictory = isVictory;
            UpdateView();
        }
        
        public void UpdateReward(Resource reward)
        {
            _reward = reward;
            UpdateMoney();
        }
        
        private void UpdateView()
        {
            _resultLabel.text = _isVictory ? "Victory" : "Defeat";
            UpdateMoney();
        }

        private void UpdateMoney()
        {
            _moneyLabel.text = _reward.Value.ToString();
        }

        public void WatchAdForDoubleMoney()
        {
            OnAdStartWatch?.Invoke();
        }

        
    }
}

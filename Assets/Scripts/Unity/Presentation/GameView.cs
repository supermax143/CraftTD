using Core.Application.Interfaces.Windows;
using TMPro;
using Unity.Game;
using Unity.Presentation.Windows;
using UnityEngine;
using Zenject;

namespace Unity.Presentation
{
    public class GameView : MonoBehaviour
    {
        
        [SerializeField] private TMP_Text _moneyTF;
        
        [Inject] private ILevelRewardAggregator _rewardAggregator;

        private void Start()
        {
            _rewardAggregator.OnMoneyChanged += UpdateMoney;
            UpdateMoney();
        }

        private void UpdateMoney()
        {
            _moneyTF.text = _rewardAggregator.Money.Value.ToString();
        }
        
        
        [Inject] private IWindowsController _windowsController;
        
        public void ShowExampleWindow()
        {
           //_windowsController.ShowWindow<ExampleWindow>( window => window.Show());
           _windowsController.ShowWindow<UpgradeWindow>(window =>
           {
               window.Initialize();
               window.Show();
           });
        }
    }
}
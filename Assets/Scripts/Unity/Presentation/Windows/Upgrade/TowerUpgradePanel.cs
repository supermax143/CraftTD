using Core.Application.Models;
using TMPro;
using Unity.Game;
using Unity.Presentation.Components;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows.Upgrade
{
    public class TowerUpgradePanel : MonoBehaviour
    {
        [SerializeField]
        private PriceButton _priceButton;
        [SerializeField]
        private TextMeshProUGUI _healthTF;
        
        [Inject] private IMainModel _mainModel;
        [Inject] private GameStats _gameStats;
        
        private EpochModel Epoch => _mainModel.Epoch;

        private void Start()
        {
            Epoch.OnTowerLevelChanged += UpdateView;
            UpdateView();
        }

        public void UpdateView()
        {
            var price = Epoch.TowerUpgradeCost;
            _priceButton.SetPrice(price);
            _healthTF.text = Epoch.TowerHealth.ToString();
        }

        public void UpgradeLevel()
        {
            Epoch.UpgradeTowerLevel();
        }
        
        private void OnDestroy()
        {
            Epoch.OnTowerLevelChanged -= UpdateView;
        }
    }
}
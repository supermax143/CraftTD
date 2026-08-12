using Core.Application.Models;
using TMPro;
using Unity.Game;
using Unity.Presentation.Components;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows.Upgrade
{
    public class FoodUpgradePanel : MonoBehaviour
    {
        [SerializeField]
        private PriceButton _priceButton;
        [SerializeField]
        private TextMeshProUGUI _speedTF;
        
        [Inject] private IMainModel _mainModel;
        [Inject] private GameStats _gameStats;
        
        private EpochModel Epoch => _mainModel.PlayerEpoch;

        private void Start()
        {
            Epoch.OnFoodProductionLevelChanged += UpdateView;
            UpdateView();
        }

        public void UpdateView()
        {
            var price = Epoch.FoodProductionUpgradeCost;//GameStats.FoodProductionSpeedCost(Epoch.FoodProductionLevel);
            _priceButton.SetPrice(new Resource(ResourceType.Money, price));
            _speedTF.text = $"{Epoch.FoodProductionSpeed.ToString()}/c";
        }

        public void UpgradeLevel()
        {
            Epoch.UpgradeFoodProduction();
        }
        
        private void OnDestroy()
        {
            Epoch.OnFoodProductionLevelChanged -= UpdateView;
        }
    }
}
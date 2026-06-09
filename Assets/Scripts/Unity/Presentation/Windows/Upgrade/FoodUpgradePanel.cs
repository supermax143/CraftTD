using Core.Application.Models;
using TMPro;
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
        
        private EpochModel Epoch => _mainModel.Epoch;
        
        public override void Initialize()
        {
           
        }
        
    }
}
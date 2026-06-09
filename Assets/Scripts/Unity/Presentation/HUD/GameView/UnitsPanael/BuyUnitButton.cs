using System;
using TMPro;
using Unity.Game;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.HUD.UnitsPanael
{
    [RequireComponent(typeof(Button))]
    public class BuyUnitButton : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Button _button;

        [SerializeField]
        private UnitTier _unitTier;
        [SerializeField]
        private TMP_Text _unitNameTF;
        [SerializeField]
        private TMP_Text _foodCostTF;
        
        [Inject] private IFoodProduction _foodProduction;
        [Inject] private EpochManager _epochManager;
        [Inject] private IGameController _gameController;
        
        private UnitEntityInfo _unitInfo;
        private int _foodCost;
        
        private void OnValidate()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _epochManager.TryGetUnitDataByTier(_unitTier, out _unitInfo);
            _foodCost = _unitInfo.FoodCost;
            _foodProduction.OnFoodChanged += UpdateBuyAvailable;
            UpdateView();
            UpdateBuyAvailable();
        }

        private void UpdateView()
        {
            _unitNameTF.text = _unitTier.ToString();
            _foodCostTF.text = _foodCost.ToString();
        }

        private void UpdateBuyAvailable()
        {
            _button.interactable = _foodProduction.FoodCount >= _foodCost;
        }

        public void BuyUnit()
        {
            _gameController.BuyUnit(_unitTier);
        }
    }
    
}
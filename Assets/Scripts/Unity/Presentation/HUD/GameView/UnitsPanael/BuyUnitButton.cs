using System;
using Core.Application.Models;
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
        [Inject] private IGameController _gameController;
        [Inject] private IMainModel _mainModel;
        
        private EpochModel Epoch => _mainModel.Epoch;
        
        private UnitModel _unit;
        private int _foodCost;

        public UnitTier Tier => _unitTier;

        private void OnValidate()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            Epoch.TryGetUnitModel(_unitTier, Faction.Player, out _unit);
            _foodCost = _unit.FoodCost;
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
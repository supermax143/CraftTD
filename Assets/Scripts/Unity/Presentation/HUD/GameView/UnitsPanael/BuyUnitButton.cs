using System;
using Core.Application.Models;
using Environments.Land.Scripts.Runtime.GUI;
using TMPro;
using Unity.Game;
using Unity.Presentation.Components;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.HUD.UnitsPanael
{
    [RequireComponent(typeof(Button))]
    public class BuyUnitButton : PointerDownClicker
    {
        [SerializeField, HideInInspector]
        private Button _button;

        [SerializeField]
        private UnitTier _unitTier;
        [SerializeField]
        private TMP_Text _foodCostTF;
        [SerializeField] 
        private UnitIcon _unitIcon;
        
        [Inject] private IFoodProduction _foodProduction;
        [Inject] private IGameController _gameController;
        [Inject] private IMainModel _mainModel;
        
        private EpochModel Epoch => _mainModel.PlayerEpoch;
        
        private UnitModel _unit;
        private int _foodCost;

        public UnitTier Tier => _unitTier;
        protected override bool Active => _foodProduction.FoodCount >= _foodCost;

        private void OnValidate()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            Epoch.TryGetUnitModel(_unitTier, out _unit);
            _foodCost = _unit.FoodCost;
            _foodProduction.OnFoodChanged += UpdateBuyAvailable;
            UpdateView();
            UpdateBuyAvailable();
        }

        private void UpdateView()
        {
            _foodCostTF.text = _foodCost.ToString();
            _unitIcon.Initialize(_unit.Info.UnitPrefab);
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
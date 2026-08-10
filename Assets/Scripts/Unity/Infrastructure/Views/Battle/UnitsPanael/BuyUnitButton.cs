using System;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
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
        
        private int _foodCost;

        public UnitTier Tier => _unitTier;
        protected override bool Active => _foodProduction.FoodCount >= _foodCost;

        private void OnValidate()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _foodProduction.OnFoodChanged += UpdateBuyAvailable;
        }

        public async UniTask UpdateView()
        {
            Epoch.TryGetUnitModel(_unitTier, out var  unit);
            _foodCost = unit.FoodCost;
            _foodCostTF.text = _foodCost.ToString();
            await _unitIcon.Initialize(unit.Info.UnitPrefab);
            UpdateBuyAvailable();
        }

        private void UpdateBuyAvailable()
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            var active = _foodProduction.FoodCount >= _foodCost;
            _button.interactable = active;
            var color = _unitIcon.Image.color;
            color.a = active ? 1f : .6f;
            _unitIcon.Image.color = color;
            if (!_unitIcon.TryGetUnitView(out var unitView))
            {
                return;
            }
            if (active)
            {
                unitView.Unpause();
            }
            else
            {
                unitView.Pause();
            }
        }

        public void BuyUnit()
        {
            _gameController.BuyUnit(_unitTier);
        }

        public void Clear()
        {
            _unitIcon.Dispose();
        }
    }
    
}
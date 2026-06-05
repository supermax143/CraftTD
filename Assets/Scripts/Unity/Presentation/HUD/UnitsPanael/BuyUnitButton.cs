using System;
using Unity.Game;
using UnityEngine;
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

        [Inject] private FoodProduction _foodProduction;
        [Inject] private EpochManager _epochManager;
       
        private UnitEntityData _unitData;
        private int _foodCost;
        
        private void OnValidate()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _epochManager.TryGetUnitDataByTier(_unitTier, out _unitData);
            _foodCost = _unitData.Cost;
            _foodProduction.OnFoodProduced += UpdateBuyAvailable;
            UpdateBuyAvailable();
        }
        
        private void UpdateBuyAvailable()
        {
            _button.interactable = _foodProduction.CurrentFoodCount >= _foodCost;
        }
        
        
    }
    
}
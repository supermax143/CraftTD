using System;
using System.Collections.Generic;
using Core.Application.Models;
using Unity.Game;
using Unity.Presentation.Components;
using Unity.Presentation.HUD.UnitsPanael;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.HUD
{
    public class UnitsPanel : MonoBehaviour
    {
        [SerializeField]
        private ProductionProgressBar productionProgressBar;
        [SerializeField] 
        private List<BuyUnitButton> _buyUnitButtons;

        [Inject]
        private readonly IMainModel _model;
        
        private EpochModel Epoch => _model.PlayerEpoch;

        private void Start()
        {
            Epoch.OnUnitOpened += UpdateButtons;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            foreach (var buyUnitButton in _buyUnitButtons)
            {
                Epoch.TryGetUnitModel(buyUnitButton.Tier, out var unit);
                buyUnitButton.gameObject.SetActive(unit.IsUnitOpened);
            }   
        }


        private void OnDestroy()
        {
            Epoch.OnUnitOpened -= UpdateButtons;
        }
    }
}
using System;
using System.Collections.Generic;
using Core.Application.Models;
using TMPro;
using Unity.Game;
using Unity.Infrastructure.Windows;
using Unity.Presentation.Components;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows
{
    [Window(nameof(UpgradeWindow))]
    public class UpgradeWindow : WindowBase
    {
        [SerializeField]
        private List<UnitOpenItem> _unitsItems;
        [SerializeField]
        private TextMeshProUGUI _moneyTF;
        
        [Inject] IMainModel _model;
        
        public EpochModel Epoch => _model.Epoch;
        
        public override void Initialize()
        {
            _moneyTF.text = _model.Money.ToString();
            UpdateUnits();
        }

        private void UpdateUnits()
        {
            foreach (var unitOpenItem in _unitsItems)
            {
                var unitModel = Epoch.GetUnitByTier(unitOpenItem.Tier);
                unitOpenItem.SetUnit(unitModel);
                unitOpenItem.OnUnitOpened += Epoch.OpenUnit;
            }
        }
        
        
    }
}
using System;
using System.Collections.Generic;
using Core.Application.Models;
using TMPro;
using Unity.Game;
using Unity.Infrastructure.Windows;
using Unity.Presentation.Components;
using Unity.Presentation.Windows.Upgrade;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows
{
    [Window(nameof(UpgradeWindow))]
    public class UpgradeWindow : WindowBase
    {
        
        [SerializeField]
        private TextMeshProUGUI _epochTF;
        [SerializeField]
        private List<UnitOpenItem> _unitsItems;
        [SerializeField]
        private TextMeshProUGUI _moneyTF;
        [SerializeField]
        private FoodUpgradePanel _foodUpgradePanel;
        [SerializeField]
        private TowerUpgradePanel _towerUpgradePanel;
        [SerializeField]
        private EpochCompletePanel _epochCompletePanel;
        
        [Inject] IMainModel _model;
        
        public EpochModel Epoch => _model.PlayerEpoch;
        
        public override void Initialize()
        {
            _model.Inventory.OnMoneyChanged += UpdateMoney;
            Epoch.OnUnitOpened += UpdateUnits;
            _epochCompletePanel.OnEpochComplete += UpdateView;
            UpdateView();
        }

        private void UpdateView()
        {
            _epochTF.text = Epoch.Name;
            UpdateMoney();
            UpdateUnits();
            _foodUpgradePanel.UpdateView();
            _towerUpgradePanel.UpdateView();
            _epochCompletePanel.UpdateView();
        }
        

        private void UpdateMoney()
        {
            _moneyTF.text = _model.Money.Value.ToString();
        }
        
        private void UpdateUnits()
        {
            foreach (var unitOpenItem in _unitsItems)
            {
                Epoch.TryGetUnitModel(unitOpenItem.Tier, out var unitModel);
                unitOpenItem.SetUnit(unitModel);
                unitOpenItem.OnUnitOpened += Epoch.OpenUnit;
            }
        }
        
        private void OnDestroy()
        {
            _model.Inventory.OnMoneyChanged -= UpdateMoney;
            Epoch.OnUnitOpened -= UpdateUnits;
            _epochCompletePanel.OnEpochComplete -= UpdateView;
        }
    }
}
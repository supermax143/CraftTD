using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Exploration.Scripts.Controllers.ModelRender;
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
        [Inject] ModelToAtlasRenderer _modelToTextureRenderer;
        
        public EpochModel Epoch => _model.PlayerEpoch;
        
        public override void Initialize()
        {
            _model.Inventory.OnMoneyChanged += UpdateMoney;
            Epoch.OnUnitOpened += OnUnitsOpened;
            _epochCompletePanel.OnEpochComplete += OnEpochComplete;
            UpdateView();
        }

        private void OnEpochComplete()
        {
            UpdateView();
        }

        private async UniTask UpdateView()
        {
            _epochTF.text = Epoch.Name;
            _modelToTextureRenderer.BlockAtlasPack();
            UpdateMoney();
            await UpdateUnits();
            _modelToTextureRenderer.UnblockAtlasPack();
            _foodUpgradePanel.UpdateView();
            _towerUpgradePanel.UpdateView();
            _epochCompletePanel.UpdateView();
            
            //TODO: в WEBGL не отображается подругому
            foreach (var unitOpenItem in _unitsItems)
            {
               unitOpenItem.gameObject.SetActive(false);
               unitOpenItem.gameObject.SetActive(true);
            }
        }

        private void UpdateMoney()
        {
            _moneyTF.text = _model.Money.Value.ToString();
        }
        
        
        
        private async UniTask UpdateUnits()
        {
            foreach (var unitOpenItem in _unitsItems)
            {
                Epoch.TryGetUnitModel(unitOpenItem.Tier, out var unitModel);
                await unitOpenItem.SetUnit(unitModel);
                unitOpenItem.OnUnitOpened += Epoch.OpenUnit;
            }
        }
        
        private void OnDestroy()
        {
            _model.Inventory.OnMoneyChanged -= UpdateMoney;
            Epoch.OnUnitOpened -= OnUnitsOpened;
            _epochCompletePanel.OnEpochComplete -= OnEpochComplete;
        }

        private void OnUnitsOpened()
        {
            _ = UpdateUnits();
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Exploration.Scripts.Controllers.ModelRender;
using TMPro;
using Unity.Game;
using Unity.Infrastructure.VisualActions;
using Unity.Infrastructure.VisualActions.ActionsData;
using Unity.Infrastructure.Windows;
using Unity.Presentation.Components;
using Unity.Presentation.Views;
using Unity.Presentation.Windows.Upgrade;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows
{
    public class UpgradeView : ViewBase
    {
        
        [SerializeField]
        private TextMeshProUGUI _epochTF;
        [SerializeField]
        private List<UnitOpenItem> _unitsItems;
        /*[SerializeField]
        private TextMeshProUGUI _moneyTF;*/
        [SerializeField]
        private FoodUpgradePanel _foodUpgradePanel;
        [SerializeField]
        private TowerUpgradePanel _towerUpgradePanel;
        [SerializeField]
        private EpochCompletePanel _epochCompletePanel;
        
        [Inject] IMainModel _model;
        [Inject] ModelToAtlasRenderer _modelToTextureRenderer;
        [Inject] IActionsDispatcher _actionsDispatcher;
        
        public EpochModel Epoch => _model.PlayerEpoch;
        
        public override void Initialize()
        {
            Epoch.OnUnitOpened += OnUnitsOpened;
            _epochCompletePanel.OnEpochComplete += OnEpochComplete;
            UpdateView();
        }

        private void OnEpochComplete()
        {
            _actionsDispatcher.AddAction(new ChangeEpochActionData(true, true));
            UpdateView();
        }

        private async UniTask UpdateView()
        {
            _epochTF.text = Epoch.Name;
            _modelToTextureRenderer.BlockAtlasPack();
            // UpdateMoney();
            _modelToTextureRenderer.UnblockAtlasPack();
            _foodUpgradePanel.UpdateView();
            _towerUpgradePanel.UpdateView();
            _epochCompletePanel.UpdateView();
            foreach (var unitOpenItem in _unitsItems)
            {
                unitOpenItem.Hide(0);
            }
            await UpdateUnits();
            //TODO: в WEBGL не отображается подругому
            /*foreach (var unitOpenItem in _unitsItems)
            {
               unitOpenItem.gameObject.SetActive(false);
               unitOpenItem.gameObject.SetActive(true);
            }*/
        }

        
        
        private async UniTask UpdateUnits()
        {
            foreach (var unitOpenItem in _unitsItems)
            {
                Epoch.TryGetUnitModel(unitOpenItem.Tier, out var unitModel);
                await unitOpenItem.SetUnit(unitModel);
                unitOpenItem.OnUnitOpened += Epoch.OpenUnit;
                unitOpenItem.gameObject.SetActive(false);
                unitOpenItem.gameObject.SetActive(true);
                unitOpenItem.Show(0.2f);
            }
        }
        
        private void OnDestroy()
        {
            // _model.Inventory.OnMoneyChanged -= UpdateMoney;
            Epoch.OnUnitOpened -= OnUnitsOpened;
            _epochCompletePanel.OnEpochComplete -= OnEpochComplete;
        }

        private void OnUnitsOpened()
        {
            _ = UpdateUnits();
        }
    }
}
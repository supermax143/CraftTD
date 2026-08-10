using System;
using System.Collections.Generic;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Exploration.Scripts.Controllers.ModelRender;
using Unity.Game;
using Unity.Presentation.Components;
using Unity.Presentation.HUD.UnitsPanael;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.HUD
{
    public class UnitsBuyPanel : MonoBehaviour
    {
        [SerializeField]
        private ProductionProgressBar productionProgressBar;
        [SerializeField] 
        private List<BuyUnitButton> _buyUnitButtons;

        [Inject] private readonly IMainModel _model;
        [Inject] private readonly ModelToAtlasRenderer _modelToTextureRenderer;
        
        private EpochModel Epoch => _model.PlayerEpoch;

        private void Start()
        {
            Epoch.OnUnitOpened += OnUnitUpdated;
            //UpdateView();
        }
        
        private void OnUnitUpdated()
        {
            UpdateView().Forget();
        }
        
        public async UniTask UpdateView()
        {
            _modelToTextureRenderer.BlockAtlasPack();
            foreach (var buyUnitButton in _buyUnitButtons)
            {
                Epoch.TryGetUnitModel(buyUnitButton.Tier, out var unit);
                buyUnitButton.gameObject.SetActive(unit.IsUnitOpened);
                if (unit.IsUnitOpened)
                {
                    await buyUnitButton.UpdateView();
                }
            }   
            _modelToTextureRenderer.UnblockAtlasPack();
            
            //TODO: в WEBGL не отображается подругому
            foreach (var buyUnitButton in _buyUnitButtons)
            {
                if (!buyUnitButton.gameObject.activeSelf)
                {
                    continue;
                }
                buyUnitButton.gameObject.SetActive(false);
                buyUnitButton.gameObject.SetActive(true);
            }   
        }

        private void OnDestroy()
        {
            Epoch.OnUnitOpened -= OnUnitUpdated;
        }


        public void Clear()
        {
            foreach (var buyUnitButton in _buyUnitButtons)
            {
                buyUnitButton.Clear();
            }  
        }
    }
}
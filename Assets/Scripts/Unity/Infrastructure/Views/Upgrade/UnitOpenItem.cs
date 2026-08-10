using System;
using System.Threading.Tasks;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Environments.Land.Scripts.Runtime.GUI;
using TMPro;
using Unity.Game;
using UnityEngine;

namespace Unity.Presentation.Components
{
    public class UnitOpenItem : MonoBehaviour
    {
        public event Action<UnitTier> OnUnitOpened;
        
        [SerializeField] private UnitIcon _unitIcon;
        [SerializeField] private PriceButton _priceButton;
        [SerializeField] private UnitTier _tier;
        private UnitModel _unitModel;

        public UnitTier Tier => _tier;

        public async UniTask SetUnit(UnitModel unitModel)
        {
            _unitModel = unitModel;
            await _unitIcon.Initialize(unitModel.Info.UnitPrefab);
            _priceButton.SetPrice(new Resource(ResourceType.Money, _unitModel.UnlockCost));
            _priceButton.gameObject.SetActive(!_unitModel.IsUnitOpened);
        }
       
        public void OpenUnit()
        {
            OnUnitOpened?.Invoke(_tier);
        }
       
    }
}
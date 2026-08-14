using System;
using Core.Application.Models;
using Unity.Presentation.Components;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.HUD
{
    public class GameHUD : MonoBehaviour
    {
        public enum State
        {
            Idle, 
            Battle
        }

        [SerializeField]
        private ResourceContainer _moneyContainer;
        [SerializeField]
        private ResourceContainer _crystalContainer;
        [SerializeField, HideInInspector]
        private GameHUDAnimatorController _animatorController;
        
        [Inject] private IInventoryModel _inventory;
        [Inject] private IMainModel _mainModel;
        
        private State _state = State.Idle;

        private void OnValidate()
        {
            _animatorController = GetComponentInChildren<GameHUDAnimatorController>();
        }

        public void Start()
        {
            _inventory.OnResourceChanged += OnResourceChanged;
            _mainModel.OnPlayerEpochChanged += UpdateResources;
            UpdateResources();
        }

        private void UpdateResources()
        {
            _moneyContainer.SetValue(_inventory.GetResourceCount(_moneyContainer.ResourceType));
            _crystalContainer.SetValue(_inventory.GetResourceCount(_crystalContainer.ResourceType));
        }
        
        private void OnResourceChanged(ResourceType resourceType)
        {
            if (resourceType == _moneyContainer.ResourceType)
            {
                _moneyContainer.SetValue(_inventory.GetResourceCount(resourceType));
            }
            
            if (resourceType == _crystalContainer.ResourceType)
            {
                _crystalContainer.SetValue(_inventory.GetResourceCount(resourceType));
            }
        }


        public void ShowBattleView()
        {
            _state = State.Battle;
            _animatorController.SetIsBattleState(true);
        }
        
        public void ShowIdleView()
        {
            _state = State.Idle;
            _animatorController.SetIsBattleState(false);
        }
        
    }
}
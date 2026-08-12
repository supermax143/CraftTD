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
        private ResourceContainer _resourceContainer;
        [SerializeField, HideInInspector]
        private GameHUDAnimatorController _animatorController;
        
        [Inject] private IInventoryModel _inventory;
        
        private State _state = State.Idle;

        private void OnValidate()
        {
            _animatorController = GetComponentInChildren<GameHUDAnimatorController>();
        }

        public void Start()
        {
            _inventory.OnResourceChanged += OnResourceChanged;
            _resourceContainer.SetValue(_inventory.GetResourceCount(_resourceContainer.ResourceType));
        }

        private void OnResourceChanged(ResourceType resourceType)
        {
            if (resourceType == _resourceContainer.ResourceType)
            {
                _resourceContainer.SetValue(_inventory.GetResourceCount(resourceType));
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
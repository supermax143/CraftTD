using System;
using Core.Application.Models;
using Cysharp.Threading.Tasks;
using Unity.Infrastructure.ResourceManager;
using Unity.Presentation.Components;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
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
        [SerializeField]
        private AssetReference _itemPrefab;
        [SerializeField]
        private Transform _itemsContainer;
        [SerializeField]
        private Transform _buttonExit;
        [SerializeField]
        private Transform _buttonSpeedup;
        
        
        
        [Inject] private IInventoryModel _inventory;
        [Inject] private IMainModel _mainModel;
        [Inject] private DiContainer _container;
        
        private State _state = State.Idle;

        private void OnValidate()
        {
            _animatorController = GetComponentInChildren<GameHUDAnimatorController>();
        }

        public void Start()
        {
#if DEBUG_MODE
            _buttonExit.gameObject.SetActive(true);
            _buttonSpeedup.gameObject.SetActive(true);
#else
            _buttonExit.gameObject.SetActive(false);
            _buttonSpeedup.gameObject.SetActive(false); 
#endif
            _inventory.OnResourceChanged += OnResourceChanged;
            _inventory.OnItemsChanged += OnItemsChanged;
            _mainModel.OnPlayerEpochChanged += UpdateResources;
            UpdateResources();
            BuildItems().Forget();
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

        private void OnItemsChanged()
        {
            BuildItems().Forget();
        }

        private async UniTask BuildItems()
        {
            foreach (Transform child in _itemsContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var item in _inventory.Items)
            {
                var prefab = await _itemPrefab.LoadAssetReference<GameObject>(gameObject);
                var view = _container.InstantiatePrefabForComponent<InventoryItemContainer>(prefab, _itemsContainer);
                await view.SetItem(item);
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

        private void OnDestroy()
        {
            _inventory.OnResourceChanged -= OnResourceChanged;
            _inventory.OnItemsChanged -= OnItemsChanged;
            _mainModel.OnPlayerEpochChanged -= UpdateResources;
        }

    }
}
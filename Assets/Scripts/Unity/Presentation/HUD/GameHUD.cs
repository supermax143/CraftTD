using System;
using System.Collections.Generic;
using Core.Application.Interfaces;
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
        [SerializeField] 
        private ResourceContainer[] _resourceContainers;
        
        [Inject] private IInventoryModel _inventory;
        [Inject] private IMainModel _mainModel;
        [Inject] private DiContainer _container;
        
        private State _state = State.Idle;

        private readonly Dictionary<ResourceType, HashSet<IBlocker>> _resourceBlockers = new();

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
        
        private bool IsResourceUpdateBlocked(ResourceType resourceType)
        {
            return _resourceBlockers.TryGetValue(resourceType, out var blockers) && blockers.Count > 0;
        }

        private void UpdateResources()
        {
            foreach (var container in _resourceContainers)
            {
                if (IsResourceUpdateBlocked(container.ResourceType))
                {
                    continue;
                }
                container.SetValue(_inventory.GetResourceCount(container.ResourceType));
            }
        }

        public void BlockResourceUpdate(IBlocker blocker, ResourceType resourceType)
        {
            if (!_resourceBlockers.TryGetValue(resourceType, out var blockers))
            {
                blockers = new HashSet<IBlocker>();
                _resourceBlockers[resourceType] = blockers;
            }
            blockers.Add(blocker);
        }

        public void UnBlockResourceUpdate(IBlocker blocker, ResourceType resourceType)
        {
            if (_resourceBlockers.TryGetValue(resourceType, out var blockers))
            {
                blockers.Remove(blocker);
            }
        }

        private void OnResourceChanged(ResourceType resourceType)
        {
           
            foreach (var container in _resourceContainers)
            {
                if (IsResourceUpdateBlocked(container.ResourceType) ||container.ResourceType != resourceType)
                {
                    continue;
                }
                container.SetValue(_inventory.GetResourceCount(container.ResourceType));
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
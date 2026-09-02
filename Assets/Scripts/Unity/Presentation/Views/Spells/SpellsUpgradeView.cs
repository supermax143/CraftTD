using System;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Application.Spells;
using Cysharp.Threading.Tasks;
using Unity.Presentation.Components;
using Unity.Presentation.Views;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Windows
{
    /// <summary>
    /// Окно для апгрейда спелов
    /// </summary>
    public class SpellsUpgradeView : ViewBase
    {
        [SerializeField] private Transform _spellsContainer;
        [SerializeField] private GameObject _spellItemPrefab;
        
        [Inject] private SpellCollection _spellCollection;
        [Inject] private DiContainer _container;
        [Inject] private IInventoryModel _inventory;
        
        
        public override void Initialize()
        {
            _inventory.OnResourceChanged += OnResourceChanged;
            BuildItems().Forget();
        }

        private async UniTask BuildItems()
        {
            return;
            foreach (var spellModel in _spellCollection.GetAllSpells())
            {
                var view = _container.InstantiatePrefabForComponent<SpellUpgradeItem>(_spellItemPrefab, _spellsContainer);
                view.OnUpgradeClicked += OnUpgradeClicked;
                view.Initialize(spellModel);
            }
        }

        private void OnUpgradeClicked(SpellModel spellModel)
        {
            if (!spellModel.TryGetNextUpgrade(out var upgrade))
            {
                return;
            }

            if (!_inventory.HasEnough(upgrade.Cost.Type, upgrade.Cost.Value))
            {
                return;
            }

            _inventory.WithdrawResource(upgrade.Cost);
            _spellCollection.UpgradeSpell(spellModel.Config.Id);
        }

        private void OnResourceChanged(ResourceType resourceType)
        {
        }

        private void OnDestroy()
        {
            _inventory.OnResourceChanged -= OnResourceChanged;
        }
    }
}

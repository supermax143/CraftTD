using System;
using Core.Application.Info.Attributes.AttributeModifiers;
using Core.Application.Models;

namespace Core.Application.Spells
{
    public class SpellModel
    {
        public event Action OnSpellUnlocked;
        public event Action OnSpellLevelChanged;

        private readonly SpellDefinition _definition;
        private int _currentLevel;
        private readonly InventoryModel _inventory;

        public SpellDefinition Definition => _definition;
        public int CurrentLevel => _currentLevel;
        public bool IsUnlocked => _currentLevel > 0;

        public SpellModel(SpellDefinition definition, InventoryModel inventory, bool isUnlocked = false, int currentLevel = 1)
        {
            _definition = definition;
            _inventory = inventory;
            _currentLevel = currentLevel;
        }

        private bool CanUnlock()
        {
            if (IsUnlocked)
            {
                return false;
            }
            var unlockCost = GetUnlockCost();
            return _inventory.HasEnough(unlockCost.Type, unlockCost.Value);
        }

        public void Unlock()
        {
            if (IsUnlocked) return;
            if (!CanUnlock()) return;

            var unlockCost = GetUnlockCost();
            _inventory.WithdrawResource(unlockCost);

            OnSpellUnlocked?.Invoke();
        }

        public bool CanUpgrade()
        {
            if (!IsUnlocked) return false;
            if (_currentLevel >= _definition.MaxLevel) return false;

            var upgradeCost = GetUpgradeCost();
            return _inventory.HasEnough(upgradeCost.Type, upgradeCost.Value);
        }

        public void Upgrade()
        {
            if (!IsUnlocked) return;
            if (_currentLevel >= _definition.MaxLevel) return;
            if (!CanUpgrade()) return;

            var upgradeCost = GetUpgradeCost();
            _inventory.WithdrawResource(upgradeCost);

            _currentLevel++;
            OnSpellLevelChanged?.Invoke();
        }

        public Resource GetUnlockCost()
        {
            var firstUpgrade = _definition.Upgrades.Count > 0 ? _definition.Upgrades[0] : null;
            if (firstUpgrade != null)
            {
                return firstUpgrade.Cost;
            }
            return Resource.Money(0);
        }

        public Resource GetUpgradeCost()
        {
            var nextLevel = _currentLevel + 1;
            var upgrade = _definition.Upgrades.Find(u => u.Level == nextLevel);
            if (upgrade != null)
            {
                return upgrade.Cost;
            }
            return Resource.Money(0);
        }

        public float GetCalculatedValue(string parameterName, float baseValue)
        {
            float calculatedValue = baseValue;

            foreach (var upgrade in _definition.Upgrades)
            {
                if (upgrade.Level > _currentLevel) continue;

                foreach (var modifierWrapper in upgrade.Modifiers)
                {
                    var modifier = modifierWrapper.GetModifier(GetHashCode());
                    if (modifier != null && modifier is AttributeModifier<float> typedModifier)
                    {
                        calculatedValue = typedModifier.Apply(calculatedValue);
                    }
                }
            }

            return calculatedValue;
        }
    }
}

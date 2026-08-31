using System;
using System.Collections.Generic;
using Core.Application.Info.Attributes.AttributeModifiers;
using Core.Application.Models;

namespace Core.Application.Spells
{
    public class SpellModel
    {
        public event Action OnSpellUnlocked;
        public event Action OnSpellLevelChanged;

        private readonly SpellConfig _config;
        private int _currentLevel = -1;

        public SpellConfig Config => _config;
        public int CurrentLevel => _currentLevel;
        public bool IsUnlocked => _currentLevel > 0;

        public SpellModel(SpellConfig config, int currentLevel)
        {
            _config = config;
            _currentLevel = currentLevel;
        }
/*
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
            if (_currentLevel >= _config.MaxLevel) return false;

            var upgradeCost = GetUpgradeCost();
            return _inventory.HasEnough(upgradeCost.Type, upgradeCost.Value);
        }

        public void Upgrade()
        {
            if (!IsUnlocked) return;
            if (_currentLevel >= _config.MaxLevel) return;
            if (!CanUpgrade()) return;

            var upgradeCost = GetUpgradeCost();
            _inventory.WithdrawResource(upgradeCost);

            _currentLevel++;
            OnSpellLevelChanged?.Invoke();
        }

        public Resource GetUnlockCost()
        {
            var firstUpgrade = _config.Upgrades.Count > 0 ? _config.Upgrades[0] : null;
            if (firstUpgrade != null)
            {
                return firstUpgrade.Cost;
            }
            return Resource.Money(0);
        }

        */

        public bool TryGetNextUpgrade(out SpellUpgrade upgrade)
        {
            upgrade = default;
            if (_currentLevel < 0)
            {
                upgrade = _config.Upgrades[0];
                return true;
            }
            var nextLevel = _currentLevel + 1;
            if (_currentLevel >= _config.MaxLevel)
            {
                return false;
            }
            upgrade = _config.Upgrades[nextLevel];
            return  true;
        }

        public IEnumerable<AttributeModifierBase> GetModifiers()
        {
            for (int i = 0; i < _config.Upgrades.Count; i++)
            {
                if (i >= _currentLevel)
                {
                    break;
                }
                var upgrade = _config.Upgrades[i];
                foreach (var modifierWrapper in upgrade.Modifiers)
                {
                    var modifier = modifierWrapper.GetModifier();
                    yield return modifier;
                }
            }
        }
        
        /*public float GetCalculatedValue(string parameterName, float baseValue)
        {
            float calculatedValue = baseValue;

            foreach (var upgrade in _config.Upgrades)
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
        }*/
    }
}

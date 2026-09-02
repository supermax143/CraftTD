using System;
using System.Collections.Generic;
using Core.Application.Info.Attributes.AttributeModifiers;
using Core.Application.Models;

namespace Core.Application.Spells
{
    public class SpellModel
    {
        public event Action OnSpellLevelChanged;
        public event Action OnSpellEquippedChanged;

        private readonly SpellConfig _config;
        private int _currentLevel = -1;
        private bool _isEquipped;

        public SpellConfig Config => _config;
        public int CurrentLevel => _currentLevel;
        public bool IsUnlocked => _currentLevel >= 0;
        public bool IsEquipped => _isEquipped;

        public SpellModel(SpellConfig config, int currentLevel, bool isEquipped = false)
        {
            _config = config;
            _currentLevel = currentLevel;
            _isEquipped = isEquipped;
        }


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
        
        public void Upgrade()
        {
            _currentLevel++;
            OnSpellLevelChanged?.Invoke();  
        }

        public void SetEquipped(bool equipped)
        {
            if (_isEquipped == equipped)
            {
                return;
            }
            _isEquipped = equipped;
            OnSpellEquippedChanged?.Invoke();
        }
        
    }
}

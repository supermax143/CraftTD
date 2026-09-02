using System.Collections.Generic;
using System.Linq;
using Core.Application.DataStorage;
using Core.Application.Models;
using Unity.Game.Spells;
using Unity.Settings;
using Zenject;

namespace Core.Application.Spells
{
    public class SpellCollection : IInitializable
    {
        
        [Inject] private SpellDatabase _spellDatabase;
        [Inject] private readonly IDataStorage _dataStorage;
        [Inject] private InventoryModel _inventory;
        [Inject] private IGameSettings _gamesSettings;
        
        private readonly Dictionary<string, SpellModel> _idToSpell = new();
        
        public SpellsStorageData Spells => _dataStorage.Spells;
        
        public void Initialize()
        {
            foreach (var spellConfig in _spellDatabase.GetAllSpells())
            {
                AddSpell(spellConfig);
            }
        }
        
        public int CurEquippedSpells() => _idToSpell.Values.Count(s => s.IsEquipped);
        
        private void AddSpell(SpellConfig spellConfig)
        {
            var level = -1;
            var isEquipped = false;
            if (Spells.TryGetSpellProgress(spellConfig.Id, out var progress))
            {
                level = progress.Level;
                isEquipped = progress.IsEquipped;
            }
            var spell = new SpellModel(spellConfig, level, isEquipped);
            _idToSpell[spell.Config.Id] = spell;
        }

        public SpellModel GetSpell(string spellId)
        {
            return _idToSpell.TryGetValue(spellId, out var spell) ? spell : null;
        }

        public IEnumerable<SpellModel> GetAllSpells()
        {
            return _idToSpell.Values.ToArray();
        }
        
        public IEnumerable<SpellModel> GetEquippedSpells()
        {
            return _idToSpell.Values.Where(s => s.IsEquipped);
        }
        
        public bool TryUpgradeSpell(string spellId)
        {
            var spell = GetSpell(spellId);
            if (spell == null || !spell.TryGetNextUpgrade(out var upgrade))
            {
                return false;
            }

            if (!_inventory.HasEnough(upgrade.Cost))
            {
                return false;
            }
            
            _inventory.WithdrawResource(upgrade.Cost);
            spell.Upgrade();
            Spells.SetSpellProgress(spell.Config.Id, spell.CurrentLevel);
            return true;
        }

        public bool TryEquipSpell(string spellId)
        {
            if (CurEquippedSpells() >= _gamesSettings.MaxEquipedSpells)
            {
                return false;
            }
            var spell = GetSpell(spellId);
            if (spell == null || !spell.IsUnlocked)
            {
                return false;
            }

            spell.SetEquipped(true);
            Spells.SetSpellEquipped(spell.Config.Id, true);
            return true;
        }

        public bool TryUnequipSpell(string spellId)
        {
            var spell = GetSpell(spellId);
            if (spell == null)
            {
                return false;
            }

            spell.SetEquipped(false);
            Spells.SetSpellEquipped(spell.Config.Id, false);
            return true;
        }
    }
}

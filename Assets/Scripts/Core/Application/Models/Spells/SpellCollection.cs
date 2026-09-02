using System.Collections.Generic;
using System.Linq;
using Core.Application.DataStorage;
using Core.Application.Models;
using Unity.Game.Spells;
using Zenject;

namespace Core.Application.Spells
{
    public class SpellCollection : IInitializable
    {
        
        [Inject] private SpellDatabase _spellDatabase;
        [Inject] private readonly IDataStorage _dataStorage;
        [Inject] private InventoryModel _inventory;
        
        private readonly Dictionary<string, SpellModel> _idToSpell;
        
        public SpellsStorageData Spells => _dataStorage.Spells;
        
        public void Initialize()
        {
            foreach (var spellConfig in _spellDatabase.GetAllSpells())
            {
                AddSpell(spellConfig);
            }
        }
        
        public SpellCollection()
        {
            _idToSpell = new Dictionary<string, SpellModel>();
        }

        public void AddSpell(SpellConfig spellConfig)
        {
            var level = -1;
            if (Spells.TryGetSpellProgress(spellConfig.Id, out var progress))
            {
                level = progress.Level;
            }
            var spell = new SpellModel(spellConfig, level);
            _idToSpell[spell.Config.Id] = spell;
        }

        public SpellModel GetSpell(string spellId)
        {
            return _idToSpell.TryGetValue(spellId, out var spell) ? spell : null;
        }

        public IEnumerable<SpellModel> GetAllSpells()
        {
            return _idToSpell.Values;
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
    }
}

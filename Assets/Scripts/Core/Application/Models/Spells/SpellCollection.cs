using System.Collections.Generic;
using System.Linq;
using Core.Application.Models;
using Unity.Game.Spells;
using Zenject;

namespace Core.Application.Spells
{
    public class SpellCollection : IInitializable
    {
        
        [Inject] private SpellDatabase _spellDatabase;
        [Inject] private InventoryModel _inventory;
        
        
        private readonly Dictionary<string, SpellModel> _idToSpell;

        
        
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
            var spell = new SpellModel(spellConfig, 1);
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

        public bool UnlockSpell(string spellId)
        {
            var spell = GetSpell(spellId);
            if (spell == null || spell.IsUnlocked) return false;

            //spell.Unlock();
            return true;
        }

        public bool UpgradeSpell(string spellId)
        {
            var spell = GetSpell(spellId);
            if (spell == null || !spell.IsUnlocked) return false;

            //spell.Upgrade();
            return true;
        }
    }
}

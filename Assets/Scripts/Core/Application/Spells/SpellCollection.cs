using System.Collections.Generic;
using System.Linq;

namespace Core.Application.Spells
{
    public class SpellCollection
    {
        private readonly Dictionary<string, SpellModel> _spells;

        public SpellCollection()
        {
            _spells = new Dictionary<string, SpellModel>();
        }

        public void AddSpell(SpellModel spell)
        {
            _spells[spell.Definition.Id] = spell;
        }

        public SpellModel GetSpell(string spellId)
        {
            return _spells.TryGetValue(spellId, out var spell) ? spell : null;
        }

        public IEnumerable<SpellModel> GetAllSpells()
        {
            return _spells.Values;
        }

        public bool UnlockSpell(string spellId)
        {
            var spell = GetSpell(spellId);
            if (spell == null || spell.IsUnlocked) return false;

            spell.Unlock();
            return true;
        }

        public bool UpgradeSpell(string spellId)
        {
            var spell = GetSpell(spellId);
            if (spell == null || !spell.IsUnlocked) return false;

            spell.Upgrade();
            return true;
        }
    }
}

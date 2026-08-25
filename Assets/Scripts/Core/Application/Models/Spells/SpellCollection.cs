using System.Collections.Generic;
using System.Linq;
using Unity.Game.Spells;
using Zenject;

namespace Core.Application.Spells
{
    public class SpellCollection : IInitializable
    {
        
        [Inject] private SpellDatabase _spellDatabase;
        
        private readonly Dictionary<string, SpellModel> _idToSpell;

        
        
        public void Initialize()
        {
        }
        
        public SpellCollection()
        {
            _idToSpell = new Dictionary<string, SpellModel>();
        }

        public void AddSpell(SpellModel spell)
        {
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

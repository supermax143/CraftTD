using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game.Spells
{
    [CreateAssetMenu(fileName = "SpellDatabase", menuName = "CraftTD/SpellDatabase", order = 1)]
    public class SpellDatabase : ScriptableObject
    {
        [SerializeField]
        private List<Core.Application.Spells.SpellDefinition> _spells;

        public List<Core.Application.Spells.SpellDefinition> Spells => _spells;

        public Core.Application.Spells.SpellDefinition GetSpell(string id)
        {
            return _spells.Find(s => s.Id == id);
        }

        public IEnumerable<Core.Application.Spells.SpellDefinition> GetAllSpells()
        {
            return _spells;
        }
    }
}

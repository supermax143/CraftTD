using Core.Application.Spells;
using UnityEngine;

namespace Unity.Game.Spells
{
    public class SpellCaster : MonoBehaviour
    {
        public void CastSpell(SpellModel spell)
        {
            if (!spell.IsUnlocked)
            {
                return;
            }
        }
    }
}
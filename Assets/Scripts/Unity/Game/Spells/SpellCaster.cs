using Core.Application.Spells;
using UnityEngine;
using Zenject;

namespace Unity.Game.Spells
{
    public class SpellCaster : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        
        private SpellController _currentSpell;
        
        public void CastSpell(SpellModel spell)
        {
            /*if (!spell.IsUnlocked)
            {
                return;
            }*/

            _currentSpell = _container.InstantiatePrefabForComponent<SpellController>(spell.Config.Prefab);
            _currentSpell.Initialize(spell);
        }
    }
}
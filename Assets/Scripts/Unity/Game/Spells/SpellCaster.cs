using System;
using Core.Application.Spells;
using UnityEngine;
using Zenject;

namespace Unity.Game.Spells
{
    public class SpellCaster : MonoBehaviour
    {
        public event Action<SpellModel> OnSpellCastComplete;
        
        [Inject] private DiContainer _container;
        
        private SpellController _currentSpell;
        
        public bool HasActiveSpell => _currentSpell != null;
        
        public bool TryCastSpell(SpellModel spell)
        {
           if(HasActiveSpell)
           {
               return false;
           }

            _currentSpell = _container.InstantiatePrefabForComponent<SpellController>(spell.Config.Prefab);
            _currentSpell.OnSpellComplete += SpellCastCompleteHandler;
            _currentSpell.Initialize(spell);
            return true;
        }

        private void SpellCastCompleteHandler(SpellController spellController)
        {
            _currentSpell.OnSpellComplete -= SpellCastCompleteHandler;
            OnSpellCastComplete?.Invoke(spellController.Model);
            _currentSpell = null;
        }

        public void CancelSpell(SpellModel spell)
        {
            if (_currentSpell != null && _currentSpell.Model == spell)
            {
                _currentSpell.Cancel();
            }
        }
    }
}
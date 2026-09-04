using System;
using System.Collections.Generic;
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
        
        private Dictionary<string, int> _spellCastCount = new();
        
        public bool TryCastSpell(SpellModel spell)
        {
            if(HasActiveSpell)
            {
               return false;
            }

            if (GetCastsCount(spell) >= spell.MaxSpellsCast)
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
            _currentSpell = null;
            var castsCount = _spellCastCount.GetValueOrDefault(spellController.Model.Config.Id, 0);
            castsCount++;
            _spellCastCount[spellController.Model.Config.Id] = castsCount;
            OnSpellCastComplete?.Invoke(spellController.Model);
        }

        public int GetCastsCount(SpellModel spell) 
            => _spellCastCount.GetValueOrDefault(spell.Config.Id, 0);

        public void CancelSpell(SpellModel spell)
        {
            if (_currentSpell != null && _currentSpell.Model == spell)
            {
                _currentSpell.Cancel();
            }
        }
        
        public void Reset()
        {
            _currentSpell?.Cancel();
            _spellCastCount.Clear();
        }
    }
}
using System;
using Core.Application.Spells;
using Unity.Game.Spells;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.HUD.SpellsPanel
{
    public class SpellCastButton : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Button _button;
        [SerializeField]
        private Image _icon;

        [Inject] private SpellCaster _spellCaster;

        private bool _isCasting;
        
        private SpellModel _spell;
        

        public void Initialize(SpellModel spell)
        {
            _spell = spell;
            _icon.sprite = spell.Config.Icon;
            _isCasting = false;
        }

        public void OnCastSpellClick()
        {
            if (_isCasting)
            {
                _spellCaster.CancelSpell(_spell);
                return;
            }
            if (!_spellCaster.TryCastSpell(_spell))
            {
                return;
            }

            _spellCaster.OnSpellCastComplete += OnSpellCastComplete;
            _isCasting = true;
        }

        private void OnSpellCastComplete(SpellModel spell)
        {
            if (_spell != spell)
            {
                return;
            }
            _spellCaster.OnSpellCastComplete -= OnSpellCastComplete;
            _isCasting = false;
        }

        private void OnDestroy()
        {
            _spellCaster.OnSpellCastComplete -= OnSpellCastComplete;
        }

        public void Dispose()
        {
            if (_isCasting)
            {
                _spellCaster.CancelSpell(_spell);
            }
        }
    }
}

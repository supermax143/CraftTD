using Core.Application.Spells;
using Unity.Game.Spells;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unity.Presentation.HUD.SpellsPanel
{
    public class SpellButton : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Button _button;
        [SerializeField]
        private Image _icon;

        [Inject] private SpellCaster _spellCaster;

        private SpellModel _spell;
        

        public void Initialize(SpellModel spell)
        {
            _spell = spell;
            _icon.sprite = spell.Config.Icon;
        }

        public void OnCastSpellClick()
        {
            _spellCaster.CastSpell(_spell);
        }

        
    }
}

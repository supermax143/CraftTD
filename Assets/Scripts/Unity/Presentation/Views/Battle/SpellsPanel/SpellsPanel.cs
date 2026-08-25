using System.Collections.Generic;
using System.Linq;
using Core.Application.Spells;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.HUD.SpellsPanel
{
    public class SpellsPanel : MonoBehaviour
    {
        [SerializeField]
        private List<SpellButton> _spellButtons;

        [Inject] private SpellCollection _spellCollection;

        private void Start()
        {
            InitializeSpells();
        }

        private void InitializeSpells()
        {
            foreach (var spellButton in _spellButtons)
            {
                spellButton.gameObject.SetActive(false);
            }
            
            var spellList = _spellCollection.GetAllSpells().ToList();

            for (int i = 0; i < _spellButtons.Count; i++)
            {
                if (i >= spellList.Count)
                {
                    break;
                }
                _spellButtons[i].Initialize(spellList[i]);
                _spellButtons[i].gameObject.SetActive(true);
            }
        }
    }
}

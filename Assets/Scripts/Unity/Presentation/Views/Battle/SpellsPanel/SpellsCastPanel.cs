using System.Collections.Generic;
using System.Linq;
using Core.Application.Spells;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.HUD.SpellsPanel
{
    public class SpellsCastPanel : MonoBehaviour
    {
        [SerializeField]
        private List<SpellCastButton> _spellButtons;

        [Inject] private SpellCollection _spellCollection;

        private void Start()
        {
            UpdateView();
        }

        public void UpdateView()
        {
            foreach (var spellButton in _spellButtons)
            {
                spellButton.gameObject.SetActive(false);
            }

            var spellList = _spellCollection.GetEquippedSpells().ToArray();

            for (int i = 0; i < _spellButtons.Count; i++)
            {
                if (i >= spellList.Length)
                {
                    break;
                }
                _spellButtons[i].Initialize(spellList[i]);
                _spellButtons[i].gameObject.SetActive(true);
            }
        }

        public void Dispose()
        {
            foreach (var button in _spellButtons)
            {
                button.Dispose();
            }
        }
    }
}

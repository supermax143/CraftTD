using TMPro;
using UnityEngine;

namespace Unity.Infrastructure.Effects.TextPoup
{
    public class TextPopup : Popup
    {
        [SerializeField]
        private TMP_Text _text;
        
        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}
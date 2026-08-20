using TMPro;
using UnityEngine;

namespace Unity.Infrastructure.Effects
{
    public class TextPopup : AnimatedPopup.AnimatedPopup
    {
        [SerializeField]
        private TMP_Text _text;
        
        public void SetText(string text)
        {
            _text.text = text;
        }
        
    }
}
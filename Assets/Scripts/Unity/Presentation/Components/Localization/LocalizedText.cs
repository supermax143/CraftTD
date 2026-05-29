using UnityEngine;
using UnityEngine.UI;

namespace Unity.Presentation.Components.Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizedText : LocalizedTextBase
    {
        [SerializeField, HideInInspector]
        private Text _textField;
        
        protected override void SetText(string text)
        {
            if (_textField != null)
                _textField.text = text;
        }

        private void OnValidate()
        {
            _textField = GetComponent<Text>();
        }
    }
}

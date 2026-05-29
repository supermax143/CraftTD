using TMPro;
using UnityEngine;

namespace Unity.Presentation.Components.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedTMP : LocalizedTextBase
    {
        [SerializeField, HideInInspector]
        private TMP_Text _textField;

        private void OnValidate()
        {
            _textField = GetComponent<TMP_Text>();
        }

        protected override void SetText(string text)
        {
            if (_textField != null)
                _textField.text = text;
        }

    }
}

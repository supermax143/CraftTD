using Core.Application.Interfaces;
using UnityEngine;
using Zenject;

namespace Unity.Presentation.Components.Localization
{
    public abstract class LocalizedTextBase : MonoBehaviour 
    {
        [SerializeField]
        private string _key;

        [Inject]
        private ILocalization _localization;

        public void SetLocale(string key)
        {
            _key = key;
            UpdateText();
        }
        

        protected abstract void SetText(string text);
        
        
        private void Start()
        {
            UpdateText();
            _localization.LanguageChanged += OnLanguageChanged;
        }

        private void OnLanguageChanged(string _)
        {
            UpdateText();
        }

        private void UpdateText()
        {
            if (string.IsNullOrEmpty(_key))
                return;
            
            SetText(_localization.Get(_key));
        }

        private void OnDestroy()
        {
            _localization.LanguageChanged -= OnLanguageChanged;
        }
    }
}
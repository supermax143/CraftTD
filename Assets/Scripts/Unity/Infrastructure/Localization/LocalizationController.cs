using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.Interfaces;
using Unity.Game.Attributes;
using UnityEngine;
using Zenject;

namespace Unity.Infrastructure.Localization
{
    
#if DEBUG_MODE
    internal class LocalizationController : ILocalization, IBootstrapStep, IInitializable
#else
    internal class LocalizationController : ILocalization, IBootstrapStep
#endif
    {

        [Inject] private IResourceManager _resourceManager;
        
        public event Action<string> LanguageChanged;

        private const string LOCALIZATION_KEY = "Localization";
        
        private readonly Dictionary<string, string> _currentLanguageTable = new();

        private string _defaultLanguageCode = "ru";

        public string CurrentLanguageCode { get; private set; }
        
        public bool Initialized { get; private set; }

        public List<string> LanguageCodes => _languageCodes;

        private List<string> _languageCodes;
        private Dictionary<string, Dictionary<string, string>> _allLanguages;

        public string GetAttributeLocale(GameEntityAttributeKind kind)
            => Get($"attribute_{kind.ToString().ToLower()}");
        
#if DEBUG_MODE
        public async void Initialize()
        {
            await Init();
        }
#endif
        public async Task Init()
        {
            var localizationText = await _resourceManager.Load<TextAsset>(LOCALIZATION_KEY, GetTag());
            Initialize(localizationText?.text);
            Debug.Log($"{this.GetType().Name} Initialized");
        }


        public void SetDefaultLanguageCode(string languageCode)
        {
            _defaultLanguageCode = languageCode;
        }
        
        public bool TryGetLanguageCodes(out IEnumerable<string> codes)
        {
            codes = _languageCodes;
            return Initialized;
        }
        
        private string GetTag()
        {
            return GetHashCode().ToString();
        }

        private void Initialize(string csvText)
        {
            if (!CSVParser.TryParse(csvText, out var data))
            {
                return;
            }

            _languageCodes = data.LanguageCodes;
            _allLanguages = data.AllLanguages;
            
            Initialized =  true;
            var code = string.IsNullOrEmpty(_defaultLanguageCode) ? 
                _languageCodes.First() :
                _defaultLanguageCode;
            SetLanguage(code);
        }

       
        public void SetLanguage(string languageCode)
        {
            if (!Initialized)
            {
                return;
            }
            
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                Debug.LogWarning("[LocalizationController] Пустой код языка. Игнорируем.");
                return;
            }

            languageCode = languageCode.Trim();

            if (!_allLanguages.TryGetValue(languageCode, out var table))
            {
                Debug.LogWarning($"[LocalizationController] Язык '{languageCode}' не найден. Остаёмся на '{CurrentLanguageCode}'.");
                return;
            }

            if (string.Equals(CurrentLanguageCode, languageCode, StringComparison.OrdinalIgnoreCase))
                return;

            _currentLanguageTable.Clear();
            foreach (var kvp in table)
            {
                _currentLanguageTable[kvp.Key] = kvp.Value;
            }

            CurrentLanguageCode = languageCode;
            
            LanguageChanged?.Invoke(languageCode);
        }

        
        public string Get(string key)
        {
            if (!Initialized || string.IsNullOrEmpty(key))
                return string.Empty;

            if (_currentLanguageTable.TryGetValue(key, out var value) && !string.IsNullOrEmpty(value))
            {
                return value;
            }

            return key;
        }

      
        public bool TryGet(string key, out string value)
        {
            if (!Initialized || string.IsNullOrEmpty(key))
            {
                value = string.Empty;
                return false;
            }

            return _currentLanguageTable.TryGetValue(key, out value);
        }

    }
}


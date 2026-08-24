using System.Collections.Generic;
using Core.Application.DataStorage;
using Core.Application.DataStorage.StorageItems;
using Newtonsoft.Json;

namespace Core.Application.Spells
{
    [System.Serializable]
    public class SpellProgressStorageDataInfo
    {
        public List<SpellProgressData> Spells = new();
    }

    public class SpellProgressStorageData
    {
        private const string SPELL_PROGRESS_KEY = "SpellProgress";

        private SpellProgressStorageDataInfo _spellProgressInfo = new SpellProgressStorageDataInfo();
        private readonly StringStorageVariable _spellProgressVariable;

        public SpellProgressStorageData(IStorageProvider storageProvider)
        {
            _spellProgressVariable = new StringStorageVariable(SPELL_PROGRESS_KEY, storageProvider);

            var progressInfo = LoadProgressInfo();
            if (progressInfo != null)
            {
                _spellProgressInfo = progressInfo;
            }
            else
            {
                InitializeDefaultData();
            }
        }

        public SpellProgressData GetSpellProgress(string spellId)
        {
            return _spellProgressInfo.Spells.Find(s => s.SpellId == spellId);
        }

        public IEnumerable<SpellProgressData> GetAllSpellProgress()
        {
            return _spellProgressInfo.Spells;
        }

        public void SetSpellProgress(SpellProgressData progress)
        {
            var existing = _spellProgressInfo.Spells.Find(s => s.SpellId == progress.SpellId);
            if (existing != null)
            {
                existing.Level = progress.Level;
            }
            else
            {
                _spellProgressInfo.Spells.Add(progress);
            }
            Save();
        }

        private void InitializeDefaultData()
        {
            _spellProgressInfo = new SpellProgressStorageDataInfo
            {
                Spells = new List<SpellProgressData>()
            };
        }

        private void Save()
        {
            _spellProgressVariable.Value = SerializeToJson();
        }

        private string SerializeToJson()
        {
            return JsonConvert.SerializeObject(_spellProgressInfo, Formatting.Indented);
        }

        private SpellProgressStorageDataInfo LoadProgressInfo()
        {
            var json = _spellProgressVariable.Value;
            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<SpellProgressStorageDataInfo>(json);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to load spell progress: {e.Message}");
                return null;
            }
        }
    }
}

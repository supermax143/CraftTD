using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Application.DataStorage.StorageItems
{
    public class TutorialStorageData
    {
        private const string TUTORIAL_DATA_KEY = "TutorialData";
        
        private readonly Dictionary<string, int> _chainsProgress = new();
        private readonly StringStorageVariable _chainProgressData;
        
        private readonly IStorageProvider _storageProvider;

        public TutorialStorageData(IStorageProvider storageProvider)
        {
            _chainProgressData = new StringStorageVariable(TUTORIAL_DATA_KEY, storageProvider);
            
            var chainsProgress = LoadChainsProgress();
            if (chainsProgress != null)
            {
                _chainsProgress = chainsProgress;
            }
        }

        public void SetChainProgress(string chainId, int progress)
        {
            _chainsProgress[chainId] = progress;
            Save();
        }

        public int GetChainProgress(string chainId)
        {
            if (!_chainsProgress.TryGetValue(chainId, out var chainProgress))
            {
                return 0;
            }
            return chainProgress;
        }
        
        private void Save()
        {
            _chainProgressData.Value = SerializeToJson();
        }
        
        private string SerializeToJson()
        {
            return JsonConvert.SerializeObject(_chainsProgress, Formatting.Indented);
        }
        
        private Dictionary<string, int> LoadChainsProgress()
        {
            var json = _chainProgressData.Value;
            if (string.IsNullOrEmpty(json))
                return null;
                
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load tutorial data: {e.Message}");
                return null;
            }
        }

        public void Reset()
        {
            _chainProgressData.Value = "";
        }
    }
}

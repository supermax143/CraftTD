using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Application.DataStorage.StorageItems
{
    public class RequirementsProgressData
    {
        private const string REQUIREMENTS_PROGRESS_KEY = "RequirementsProgress";
        
        private readonly Dictionary<string, int> _requirementsProgress = new();
        private readonly StringStorageVariable _requirementsProgressData;
        
        private readonly IStorageProvider _storageProvider;

        public RequirementsProgressData(IStorageProvider storageProvider)
        {
            _requirementsProgressData = new StringStorageVariable(REQUIREMENTS_PROGRESS_KEY, storageProvider);
            
            var requirementsProgress = LoadRequirementsProgress();
            if (requirementsProgress != null)
            {
                _requirementsProgress = requirementsProgress;
            }
        }

        
        public void ClearRequirementProgress(string requirementId)
        {
            _requirementsProgress.Remove(requirementId);
            Save();
        }
        
        public void SetRequirementProgress(string requirementId, int progress)
        {
            _requirementsProgress[requirementId] = progress;
            Save();
        }

        public int GetRequirementProgress(string requirementId)
        {
            if (!_requirementsProgress.TryGetValue(requirementId, out var requirementProgress))
            {
                return 0;
            }
            return requirementProgress;
        }
        
        private void Save()
        {
            _requirementsProgressData.Value = SerializeToJson();
        }
        
        private string SerializeToJson()
        {
            return JsonConvert.SerializeObject(_requirementsProgress, Formatting.Indented);
        }
        
        private Dictionary<string, int> LoadRequirementsProgress()
        {
            var json = _requirementsProgressData.Value;
            if (string.IsNullOrEmpty(json))
                return null;
                
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load requirements progress data: {e.Message}");
                return null;
            }
        }

        public void Reset()
        {
            _requirementsProgressData.Value = "";
        }
    }
}

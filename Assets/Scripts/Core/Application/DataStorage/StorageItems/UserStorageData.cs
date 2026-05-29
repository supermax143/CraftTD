using Newtonsoft.Json;
using UnityEngine;

namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// JSON-serializable structure for user data storage.
    /// Contains money and weapon levels.
    /// </summary>
    [System.Serializable]
    public class UserDataInfo
    {
        public uint Money;
    }

    /// <summary>
    /// Manages user data persistence using JSON serialization.
    /// Follows same pattern as TutorialStorageData.
    /// </summary>
    internal class UserStorageData
    {
        private const string USER_DATA_KEY = "UserData";
        
        private UserDataInfo _userDataInfo = new UserDataInfo();
        private readonly StringStorageVariable _userDataVariable;
        
        private readonly IStorageProvider _storageProvider;

        public UserStorageData(IStorageProvider storageProvider)
        {
            _userDataVariable = new StringStorageVariable(USER_DATA_KEY, storageProvider);
            
            var userDataInfo = LoadUserDataInfo();
            if (userDataInfo != null)
            {
                _userDataInfo = userDataInfo;
            }
            else
            {
                InitializeDefaultData();
            }
        }

        public uint Money
        {
            get => _userDataInfo.Money;
            set
            {
                _userDataInfo.Money = value;
                Save();
            }
        }

        

        public void AddMoney(uint amount)
        {
            _userDataInfo.Money += amount;
            Save();
        }

        public void SetMoney(uint amount)
        {
            _userDataInfo.Money = amount;
            Save();
        }

        public void Reset()
        {
            InitializeDefaultData();
            Save();
        }

        private void InitializeDefaultData()
        {
            _userDataInfo = new UserDataInfo
            {
                Money = 0,
            };
        }

        private void Save()
        {
            _userDataVariable.Value = SerializeToJson();
        }
        
        private string SerializeToJson()
        {
            return JsonConvert.SerializeObject(_userDataInfo, Formatting.Indented);
        }
        
        private UserDataInfo LoadUserDataInfo()
        {
            var json = _userDataVariable.Value;
            if (string.IsNullOrEmpty(json))
                return null;
                
            try
            {
                return JsonConvert.DeserializeObject<UserDataInfo>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load user data: {e.Message}");
                return null;
            }
        }
        
    }
}

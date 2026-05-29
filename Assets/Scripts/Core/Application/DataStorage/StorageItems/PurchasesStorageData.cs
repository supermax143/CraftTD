using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Application.DataStorage.StorageItems
{
    /// <summary>
    /// Manages purchases persistence using JSON serialization.
    /// Follows same pattern as other storage data classes.
    /// </summary>
    [System.Serializable]
    public class PurchasesInfo
    {
        public Dictionary<string, int> Purchases = new Dictionary<string, int>();
        //public bool HasPurchases => Purchases.Sum(p => p.Value) > 0;
    }

    public class PurchasesStorageData
    {
        private const string PURCHASES_KEY = "Purchases";
        
        private PurchasesInfo _purchasesInfo = new PurchasesInfo();
        private readonly StringStorageVariable _purchasesData;
        
        private readonly IStorageProvider _storageProvider;

        public PurchasesStorageData(IStorageProvider storageProvider)
        {
            _purchasesData = new StringStorageVariable(PURCHASES_KEY, storageProvider);
            
            var purchasesInfo = LoadPurchasesInfo();
            if (purchasesInfo != null)
            {
                _purchasesInfo = purchasesInfo;
            }
        }

        public void AddPurchase(string id)
        {
            if (!_purchasesInfo.Purchases.ContainsKey(id))
                _purchasesInfo.Purchases[id] = 0;
            
            _purchasesInfo.Purchases[id]++;
            Save();
        }

        public int GetPurchase(string id)
        {
            if (!_purchasesInfo.Purchases.TryGetValue(id, out var count))
                return 0;
            return count;
        }

        public bool HasAnyPurchases()
        {
            return _purchasesInfo.Purchases.Sum(p => p.Value) > 0;;
        }

        public Dictionary<string, int> GetAllPurchases()
        {
            return new Dictionary<string, int>(_purchasesInfo.Purchases);
        }

        public void Reset()
        {
            _purchasesInfo.Purchases.Clear();
            Save();
        }

        private void Save()
        {
            _purchasesData.Value = SerializeToJson();
        }
        
        private string SerializeToJson()
        {
            return JsonConvert.SerializeObject(_purchasesInfo, Formatting.Indented);
        }
        
        private PurchasesInfo LoadPurchasesInfo()
        {
            var json = _purchasesData.Value;
            if (string.IsNullOrEmpty(json))
                return null;
                
            try
            {
                return JsonConvert.DeserializeObject<PurchasesInfo>(json);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load purchases data: {e.Message}");
                return null;
            }
        }
    }
}

using System;
using Core.Application.Info.Inventory;
using UnityEngine;

namespace Core.Application.Models
{
    [Serializable]
    public class InventoryItem
    {
        [SerializeField] private string _id;
        [SerializeField] private InventoryItemConfig _config;

        public string Id => _id;
        public InventoryItemConfig Config => _config;

        public InventoryItem(string id, InventoryItemConfig config)
        {
            _id = id;
            _config = config;
        }
        
    }
}

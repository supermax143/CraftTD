using System;
using Core.Application.Info.Inventory;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Application.Models
{
    [Serializable]
    public class InventoryItem
    {
        [SerializeField] private int _id;
        [SerializeField] private InventoryItemConfig _config;

        public int Id => _id;
        public InventoryItemType Type => _config.Type;
        public string Label => _config.Label;
        public string Description => _config.Description;
        public AssetReference Icon => _config.Icon;
        public InventoryItemConfig Config => _config;

        public InventoryItem(int id, InventoryItemConfig config)
        {
            _id = id;
            _config = config;
        }
        
    }
}

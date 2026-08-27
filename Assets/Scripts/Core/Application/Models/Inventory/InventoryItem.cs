using System;
using System.Collections.Generic;
using Core.Application.Info.Attributes.AttributeModifiers;
using Core.Application.Info.Inventory;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Application.Models
{
    public class InventoryItem
    {
        private int _id;
        private InventoryItemConfig _config;
        private readonly List<AttributeModifierBase> _modifiers = new();
        
        
        public int Id => _id;
        public InventoryItemType Type => _config.Type;
        public string Label => _config.Label;
        public string Description => _config.Description;
        public AssetReference Icon => _config.Icon;
        public InventoryItemConfig Config => _config;

        public List<AttributeModifierBase> Modifiers => _modifiers;

        public InventoryItem(int id, InventoryItemConfig config)
        {
            _id = id;
            _config = config;
            foreach (var modifierWrapper in config.Modifiers)
            {
                _modifiers.Add(modifierWrapper.GetModifier());
            }
        }
        
    }
}

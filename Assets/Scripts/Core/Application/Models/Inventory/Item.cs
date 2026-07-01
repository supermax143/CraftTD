using System;
using UnityEngine;

namespace Core.Application.Models
{
    [Serializable]
    public class Item
    {
        [SerializeField] private string _id;

        [SerializeField] private ItemType _type;

        [SerializeField] private string _label;

        [SerializeField] private string _description;

        public string Id => _id;
        public ItemType Type => _type;
        public string Label => _label;
        public string Description => _description;

        
        public Item(string id, ItemType type, string label, string description)
        {
            _id = id;
            _type = type;
            _label = label;
            _description = description;
        }
    }
}

using System;

namespace Core.Application.Models
{
    [Serializable]
    public class Item
    {
        public string Id;
        public ItemType Type;
        public string Label;
        public string Description;

        public Item(string id, ItemType type, string label, string description)
        {
            Id = id;
            Type = type;
            Label = label;
            Description = description;
        }
    }
}

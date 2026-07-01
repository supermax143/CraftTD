using System;
using UnityEngine;

namespace Core.Application.Models
{
    /// <summary>
    /// Struct representing a game resource with type and value
    /// </summary>
    [Serializable]
    public struct Resource
    {
        public ResourceType Type;
        public int Value;

        public Resource(ResourceType type, int value)
        {
            Type = type;
            Value = value;
        }

        public static Resource Money(int value) => new Resource(ResourceType.Money, value);
        public static Resource Food(int value) => new Resource(ResourceType.Food, value);
        public static Resource Crystal(int value) => new Resource(ResourceType.Crystal, value);

        public static Resource operator +(Resource a, Resource b)
        {
            if (a.Type != b.Type)
                throw new InvalidOperationException($"Cannot add resources of different types: {a.Type} and {b.Type}");
            return new Resource(a.Type, a.Value + b.Value);
        }

        public static Resource operator -(Resource a, Resource b)
        {
            if (a.Type != b.Type)
                throw new InvalidOperationException($"Cannot subtract resources of different types: {a.Type} and {b.Type}");
            return new Resource(a.Type, a.Value - b.Value);
        }

        public static bool operator ==(Resource a, Resource b)
        {
            return a.Type == b.Type && a.Value == b.Value;
        }

        public static bool operator !=(Resource a, Resource b)
        {
            return !(a == b);
        }

        public static bool operator <(Resource a, Resource b)
        {
            if (a.Type != b.Type)
                throw new InvalidOperationException($"Cannot compare resources of different types: {a.Type} and {b.Type}");
            return a.Value < b.Value;
        }

        public static bool operator >(Resource a, Resource b)
        {
            if (a.Type != b.Type)
                throw new InvalidOperationException($"Cannot compare resources of different types: {a.Type} and {b.Type}");
            return a.Value > b.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is Resource other)
                return this == other;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Value);
        }

        public override string ToString()
        {
            return $"{Type}: {Value}";
        }
    }
}

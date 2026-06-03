using System;
using UnityEngine;

namespace Unity.Game.Attributes
{
    [Serializable]
    public abstract class GameEntityAttribute
    {
        public abstract GameEntityAttributeKind Kind { get; }
    }
    
    [Serializable]
    public abstract class GameEntityAttribute<TValue> : GameEntityAttribute
    {
        [SerializeField]
        private TValue _value;

        public TValue Value => _value;
        public override GameEntityAttributeKind Kind { get; }
    
        protected GameEntityAttribute(TValue value, GameEntityAttributeKind kind)
        {
            _value = value;
            Kind = kind;
        }
    }
}
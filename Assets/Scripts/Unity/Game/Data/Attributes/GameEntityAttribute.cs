using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unity.Game.Attributes
{
    [Serializable]
    public abstract class GameEntityAttribute
    {
        public abstract GameEntityAttributeKind Kind { get; }
        
        protected abstract object GetValueObject();
        protected abstract void SetValueObject(object value);

        public void CopyValueFrom(GameEntityAttribute other)
        {
            if (other == null) return;
            if (other.GetType() != GetType()) return;

            SetValueObject(other.GetValueObject());
        }
        
    }
    
    [Serializable]
    public abstract class GameEntityAttribute<TValue> : GameEntityAttribute
    {
        [HideLabel]
        [SerializeField]
        private TValue _value;

        public TValue Value
        {
            get => _value;
            set
            {
                _value = value;
            }
        }
        
        public override GameEntityAttributeKind Kind { get; }
    
        protected GameEntityAttribute(TValue value, GameEntityAttributeKind kind)
        {
            _value = value;
            Kind = kind;
        }

        protected override object GetValueObject()
        {
            return  _value;
        }

        protected override void SetValueObject(object newValue)
        {
            if (newValue is TValue typedValue)
            {
                _value = typedValue;
            }
        }
        
    }
}
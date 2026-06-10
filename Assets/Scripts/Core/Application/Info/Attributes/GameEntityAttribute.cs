using System;
using System.Collections.Generic;
using Core.Application.Info.Attributes.AttrimuteModdifiers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unity.Game.Attributes
{
    [Serializable]
    public abstract class GameEntityAttribute
    {
        protected readonly List<AttributeModifierBase> _modifiers = new();
        
        public abstract GameEntityAttributeKind Kind { get; }
        
        protected abstract object GetValueObject();
        protected abstract void SetValueObject(object value);
        protected abstract void OnModifiersChanged();

        public void CopyValueFrom(GameEntityAttribute other)
        {
            if (other == null) return;
            if (other.GetType() != GetType()) return;

            SetValueObject(other.GetValueObject());
        }

        public virtual void AddModifier(AttributeModifierBase modifier)
        {
            _modifiers.Add(modifier);
            OnModifiersChanged(); 
        }
        
        public virtual void RemoveModifier(AttributeModifierBase modifier)
        {
            _modifiers.Remove(modifier);
            OnModifiersChanged(); 
        }


    }
    
    [Serializable]
    public abstract class GameEntityAttribute<TValue> : GameEntityAttribute
    {
        [HideLabel]
        [SerializeField]
        private TValue _value;

        private TValue _valueModified;
        private bool _isDirty;

        public override GameEntityAttributeKind Kind { get; }

        public TValue Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<TValue>.Default.Equals(_value, value))
                {
                    return;
                }
                
                _value = value;
                _isDirty = true; 
            }
        }
        
        public TValue ValueModified
        {
            get
            {
                if (_isDirty)
                {
                    RecalculateValueModified();
                }
                return _valueModified;
            }
        }
        
        protected override void OnModifiersChanged()
        {
            _isDirty = true; // Список модификаторов изменился, помечаем кэш как невалидный
        }
        
        private void RecalculateValueModified()
        {
            var currentValue = _value;
            
            foreach (var modifier in _modifiers)
            {
                if (modifier is AttributeModifier<TValue> typedModifier && typedModifier.Kind == Kind)
                {
                    currentValue = typedModifier.Apply(currentValue);
                }
            }
            
            _valueModified = currentValue;
            _isDirty = false;
        }
        
    
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
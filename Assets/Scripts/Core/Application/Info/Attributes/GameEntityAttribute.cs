using System;
using System.Collections.Generic;
using Core.Application.Info.Attributes.AttributeModifiers;
using ModestTree;
using UnityEngine;

namespace Unity.Game.Attributes
{
    [Serializable]
    public abstract class GameEntityAttribute
    {
        private List<AttributeModifierBase> _modifiers = new();
        
        public abstract GameEntityAttributeKind Kind { get; }

        public List<AttributeModifierBase> Modifiers
        {
            get
            {
                if (_modifiers == null)
                {
                    _modifiers = new();
                }
                return _modifiers;
            }
        }

        protected abstract object GetValueObject();
        protected abstract void SetValueObject(object value);
        protected abstract void OnModifiersChanged();

        
        
        public void CopyValueFrom(GameEntityAttribute other)
        {
            if (other == null) return;
            if (other.GetType() != GetType()) return;

            SetValueObject(other.GetValueObject());
            Modifiers.Clear();
            foreach (var modifier in other.Modifiers)
            {
                AddModifier(modifier);
            }
        }

        public virtual void AddModifier(AttributeModifierBase modifier)
        {
            if (modifier.AttributeKind != Kind)
            {
                return;
            }
            Modifiers.Add(modifier);
            OnModifiersChanged();
        }
        
        public void ClearModifiers()
        {
            Modifiers.Clear();
            OnModifiersChanged();
        }
        
    }

    [Serializable]
    public abstract class GameEntityAttribute<TValue> : GameEntityAttribute
    {
        
        public event Action<TValue,TValue> OnModifiedValueChanged;
        
        [SerializeField] 
        private TValue _value;

        private bool _isDirty;

        private TValue _baseValueModified;


        protected GameEntityAttribute(TValue baseValue) : base()
        {
            _value = baseValue;
        }

        public TValue BaseValue
        {
            get => _value;
            set
            {
                _value = value;
                _isDirty = true;
            }
        }
        
        public TValue BaseBaseValueModified
        {
            get
            {
                if (Modifiers.IsEmpty())
                {
                    return _value;
                }
                
                if (_isDirty) 
                    RecalculateValueModified();
                return _baseValueModified;
            }
        }

        protected override void OnModifiersChanged()
        {
            _isDirty = true; // Список модификаторов изменился, помечаем кэш как невалидный
            var oldValue = _baseValueModified;
            var newValue = BaseBaseValueModified;
            OnModifiedValueChanged?.Invoke(oldValue, newValue);
        }

        private void RecalculateValueModified()
        {
            var currentValue = _value;

            foreach (var modifier in Modifiers)
                if (modifier is AttributeModifier<TValue> typedModifier && typedModifier.AttributeKind == Kind)
                    currentValue = typedModifier.Apply(currentValue);

            _baseValueModified = currentValue;
            _isDirty = false;
        }

        protected override object GetValueObject()
        {
            return _value;
        }

        protected override void SetValueObject(object newValue)
        {
            if (newValue is TValue typedValue) _value = typedValue;
        }
    }
}
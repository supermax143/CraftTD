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
            if (TryGetModifier(modifier.ID, out var curModifier))
            {
                curModifier.Replace(modifier);
                return;
            }
            
            Modifiers.Add(modifier);
            OnModifiersChanged();
        }

        public virtual void RemoveModifier(int id)
        {
            if (!TryGetModifier(id, out var modifier))
            {
                return;
            }
            
            Modifiers.Remove(modifier);
            OnModifiersChanged();
        }

        private bool TryGetModifier(int id, out AttributeModifierBase result)
        {
            result = default;
            foreach (var modifier in Modifiers)
            {
                if (modifier.ID != id)
                {
                    continue;
                }
                result = modifier;
                return  true;
            }

            return false;
        }
        
        public virtual void RemoveModifier(AttributeModifierBase modifier)
        {
            Modifiers.Remove(modifier);
            OnModifiersChanged();
        }
    }

    [Serializable]
    public abstract class GameEntityAttribute<TValue> : GameEntityAttribute
    {
        [SerializeField] 
        private TValue _value;

        private bool _isDirty;

        private TValue _valueModified;


        protected GameEntityAttribute(TValue value) : base()
        {
            _value = value;
        }
        
        public TValue ValueModified
        {
            get
            {
                if (Modifiers.IsEmpty())
                {
                    return _value;
                }
                
                if (_isDirty) 
                    RecalculateValueModified();
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

            foreach (var modifier in Modifiers)
                if (modifier is AttributeModifier<TValue> typedModifier && typedModifier.AttributeKind == Kind)
                    currentValue = typedModifier.Apply(currentValue);

            _valueModified = currentValue;
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
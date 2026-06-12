using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Application.Info.Attributes.AttrimuteModdifiers;
using Unity.Game.Attributes;

namespace Unity.Game
{
    
    public abstract class GameEntityData
    {
        private List<GameEntityAttribute> _attributes;

        public virtual IEnumerable<GameEntityAttribute> GetAllAttributes()
        {
            if (_attributes == null)
            {
                _attributes = GetAllAttributesFromReflection().ToList();
            }
            return _attributes;
        }

        public void RefreshAttributes()
        {
            _attributes = null;
        }
        
        private IEnumerable<GameEntityAttribute> GetAllAttributesFromReflection()
        {
            var fields = GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            foreach (var fieldInfo in fields)
            {
                var attribute = fieldInfo.GetValue(this) as GameEntityAttribute;
                if (attribute != null)
                {
                    yield return attribute;
                }
            }
        }

        public bool TryGetAttribute<T>(out T attribute) where T : GameEntityAttribute
        {
            foreach (var attr in GetAllAttributes())
            {
                if (attr is T typedAttr)
                {
                    attribute = typedAttr;
                    return true;
                }
            }

            attribute = default;
            return false;
        }
        
        
        public void AddModifier(AttributeModifierBase modifier)
        {
            foreach (var attr in GetAllAttributes())
            {
                if (attr.Kind == modifier.Kind)
                {
                    attr.AddModifier(modifier);
                    break;
                }
            }
        }
    }
}
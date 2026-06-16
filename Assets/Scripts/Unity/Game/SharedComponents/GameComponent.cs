using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Game.Attributes;
using UnityEngine;

namespace Unity.Game
{
    public abstract class GameComponent : MonoBehaviour
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

        
        private IEnumerable<GameEntityAttribute> GetAllAttributesFromReflection()
        {
            var type = GetType();
            while (type != null)
            {
                var fields = type.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
                foreach (var fieldInfo in fields)
                {
                    var attribute = fieldInfo.GetValue(this) as GameEntityAttribute;
                    if (attribute != null)
                    {
                        yield return attribute;
                    }
                }
                type = type.BaseType;
            }
        }
        

        public virtual void SetData(GameEntityData data)
        {
            if (data == null)
            {
                Debug.LogError($"{this.GetType().Name} SetData: data is null");
                return;
            }
            CopyAttributes(data.GetAllAttributes());
        }

        public void CopyAttributes(IEnumerable<GameEntityAttribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                CopyAttributeIfExist(attribute);
            }
        }
        
        public bool TryGetAttribute<T>(out T attribute) where T : GameEntityAttribute
        {
            var allAttributes = GetAllAttributes();
            foreach (var attr in allAttributes)
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

        public void CopyAttributeIfExist(GameEntityAttribute copy)
        {
            foreach (var attribute in GetAllAttributes())
            {
                if (attribute.GetType() != copy.GetType())
                {
                    continue;
                }
                attribute.CopyValueFrom(copy);
                break;
            }
        }
        
    }
}

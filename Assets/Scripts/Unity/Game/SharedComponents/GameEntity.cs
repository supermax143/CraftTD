using System.Collections;
using System.Collections.Generic;
using Unity.Game.Attributes;
using UnityEngine;

namespace Unity.Game
{
    public abstract class GameEntity : MonoBehaviour
    {
        public abstract GameEntityAttribute[] GetAllAttributes();


        public void SetData(GameEntityData data)
        {
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
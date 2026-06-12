using System;
using Unity.Game.Attributes;

namespace Core.Application.Info.Attributes.AttributeModifiers
{
    [Serializable]
    public class AttributeModifierWrapper
    {
        private GameEntityAttributeKind _attributeKind;
        private GameEntityAttributeKind _modifierKind;
        
    }
}
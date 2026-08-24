using System;
using System.Collections.Generic;
using Core.Application.Info.Attributes.AttributeModifiers;
using Core.Application.Models;
using UnityEngine;

namespace Core.Application.Spells
{
    [Serializable]
    public class SpellUpgrade
    {
        [SerializeField]
        private int _level;
        [SerializeField]
        public Resource _cost;
        [SerializeField]
        public List<AttributeModifierWrapper> _modifiers;
        
        public int Level => _level;
        public Resource Cost => _cost;
        public List<AttributeModifierWrapper> Modifiers => _modifiers;
    }
}

using System;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{
    [Serializable]
    public class AttributesData
    {
        [SerializeField]
        private HealthAttribute _health;
        [SerializeField]
        private NameAttribute _name;
        
        
    }
}
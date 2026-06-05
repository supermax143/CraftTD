using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{
    [Serializable]
    public class TowerEntityData : GameEntityData
    {
        
        [InlineProperty, SerializeField] 
        private HealthAttribute _health;
        [InlineProperty, SerializeField] 
        private TowerPrefabAttribute _towerPrefab;
       
    }
}
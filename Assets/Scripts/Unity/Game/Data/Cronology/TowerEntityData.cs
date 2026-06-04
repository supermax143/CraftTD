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
        public HealthAttribute _health;
        [InlineProperty, SerializeField] 
        public TowerPrefabAttribute _towerPrefab;
        
        
        public override IEnumerable<GameEntityAttribute> GetAllAttributes()
        {
            throw new System.NotImplementedException();
        }
    }
}
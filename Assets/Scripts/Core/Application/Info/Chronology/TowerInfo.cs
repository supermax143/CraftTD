using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{
    [Serializable]
    public class TowerInfo
    {
        
        [InlineProperty, SerializeField] 
        private TowerPrefabAttribute _towerPrefab;//TODO: Сделать обычным префабом
       
        /*private HealthAttribute _health = new();

        public void Initialize(GameStats gameStats, uint epoch, Faction faction)
        {
            _health = new HealthAttribute(gameStats.GetTowerHealth(epoch, faction));
            RefreshAttributes();
        }*/

        public GameObject TowerPrefab => _towerPrefab.ValueModified;
    }
}
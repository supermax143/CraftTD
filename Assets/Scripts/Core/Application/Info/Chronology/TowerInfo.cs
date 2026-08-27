using System;
using System.Collections.Generic;
using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{
    [Serializable]
    public class TowerInfo
    {
        
        [SerializeField] 
        private TowerPrefabAttribute _towerPrefab;//TODO: Сделать обычным префабом
        

        public GameObject TowerPrefab => _towerPrefab.BaseBaseValueModified;
    }
}
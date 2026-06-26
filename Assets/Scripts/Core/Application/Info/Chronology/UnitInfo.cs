using System;
using Unity.Game.Attributes.Specific;
using UnityEngine;

namespace Unity.Game
{

    [Serializable]
    public class UnitInfo
    {
        [SerializeField] 
        private string _name;
        [SerializeField] 
        private UnitTier _tier;

        [SerializeField] 
        private UnitPrefabAttribute _unitPrefab;//TODO: заменить на обычное поле.
        
        
        public string Name => _name;
        public UnitTier Tier => _tier;
        public GameObject UnitPrefab => _unitPrefab.ValueModified;
        
    }
}

using System;
using Unity.Game.Attributes.Specific;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
        private AssetReference _unitPrefab;
        
        public string Name => _name;
        public UnitTier Tier => _tier;
        public AssetReference UnitPrefab => _unitPrefab;
        
    }
}

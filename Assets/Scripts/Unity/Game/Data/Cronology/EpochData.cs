using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unity.Game
{
    
    [Serializable]
    public class EpochData
    {
        [SerializeField]
        private string _epochName;
        
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private TowerEntityData _tower;
        
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private UnitEntityData _unitTier1;
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private UnitEntityData _unitTier2;
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private UnitEntityData _unitTier3;

        public TowerEntityData Tower => _tower;


        public UnitEntityData GetRandomUnitTier()
        {
            var random = UnityEngine.Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    return _unitTier1;
                case 1:
                    return _unitTier2;
                case 2:
                    return _unitTier3;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
    }
}
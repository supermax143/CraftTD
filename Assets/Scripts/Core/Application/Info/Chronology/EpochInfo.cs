using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.Info.Attributes.AttributeModifiers;
using UnityEngine;

namespace Unity.Game
{
    
    [Serializable]
    public class EpochInfo
    {
        [SerializeField]
        private string _epochName;
        
        [SerializeField]
        private TowerInfo _tower;
        
        [SerializeField]
        private GameObject _locationPrefab;
        
        [SerializeField]
        private UnitInfo _unitTier1;
        [SerializeField]
        private UnitInfo _unitTier2;
        [SerializeField]
        private UnitInfo _unitTier3;

        [SerializeField]
        private List<UnitWave> _waves;
        
        [SerializeField]
        private List<AttributeModifierWrapper> _unitTier1Modifiers;
        [SerializeField]
        private List<AttributeModifierWrapper> _unitTier2Modifiers;
        [SerializeField]
        private List<AttributeModifierWrapper> _unitTier3Modifiers;
        
        public TowerInfo Tower => _tower;
        
        public GameObject LocationPrefab => _locationPrefab;
        
        public List<UnitWave> Waves => _waves;

        public string EpochName => _epochName;
        
        public UnitInfo GetUnitDataByTier(UnitTier tier)
        {
            switch (tier)
            {
                case UnitTier.Tier1:
                    return _unitTier1;
                case UnitTier.Tier2:
                    return _unitTier2;
                case UnitTier.Tier3:
                    return _unitTier3;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tier), tier, null);
            }
        }
        
        public UnitTier GetRandomUnitTier()
        {
            var random = UnityEngine.Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    return UnitTier.Tier1;
                case 1:
                    return UnitTier.Tier2;
                case 2:
                    return UnitTier.Tier3;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public IEnumerable<UnitInfo> GetUnits()
        {
            yield return _unitTier1;
            yield return _unitTier2;
            yield return _unitTier3;
        }
        
        public IEnumerable<AttributeModifierWrapper> GetUnitModifiers(UnitTier tier)
        {
            return tier switch
            {
                UnitTier.Tier1 => _unitTier1Modifiers ?? Enumerable.Empty<AttributeModifierWrapper>(),
                UnitTier.Tier2 => _unitTier2Modifiers ?? Enumerable.Empty<AttributeModifierWrapper>(),
                UnitTier.Tier3 => _unitTier3Modifiers ?? Enumerable.Empty<AttributeModifierWrapper>(),
                _ => Enumerable.Empty<AttributeModifierWrapper>()
            };
        }

        
    }
}

using System;
using System.Collections.Generic;
using Core.Application.Interfaces.Info;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unity.Game
{
    
    [Serializable]
    public class EpochInfo : IEpochInfo
    {
        [SerializeField]
        private string _epochName;
        
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private TowerEntityInfo _tower;
        
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private UnitEntityInfo _unitTier1;
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private UnitEntityInfo _unitTier2;
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private UnitEntityInfo _unitTier3;

        [ListDrawerSettings(ShowIndexLabels = true)]
        [SerializeField]
        private List<UnitWave> _waves;
        
        public TowerEntityInfo Tower => _tower;
        public List<UnitWave> Waves => _waves;

        public string EpochName => _epochName;
        
        public UnitEntityInfo GetUnitDataByTier(UnitTier tier)
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
        
        public IEnumerable<IUnitInfo> GetUnits()
        {
            yield return _unitTier1;
            yield return _unitTier2;
            yield return _unitTier3;
        }

        
    }
}
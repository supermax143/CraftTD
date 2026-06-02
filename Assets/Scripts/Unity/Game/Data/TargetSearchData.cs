using System;
using UnityEngine;

namespace Unity.Game
{
    [Serializable]
    public class TargetSearchData
    {
        [SerializeField] 
        private float _detectionRange;
        [SerializeField] 
        private float _detectionInterval;
        
        public float DetectionRange => _detectionRange;
        public float DetectionInterval => _detectionInterval;

        public TargetSearchData Clone()
        {
            return new TargetSearchData
            {
                _detectionRange = _detectionRange,
                _detectionInterval = _detectionInterval
            };
        }
    }
}
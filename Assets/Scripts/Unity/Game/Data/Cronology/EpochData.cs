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
        private UnitEntityData _unitTier1;
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private UnitEntityData _unitTier2;
        [HideLabel]
        [InlineProperty]
        [SerializeField]
        private UnitEntityData _unitTier3;
    }
}
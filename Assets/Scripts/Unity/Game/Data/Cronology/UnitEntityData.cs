using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Game
{

    [Serializable]
    public class UnitEntityData : GameEntityData
    {
        [InlineProperty, SerializeField] 
        private HealthAttribute _health;
        [InlineProperty, SerializeField] 
        private MoveSpeedAttribute _moveSpeed;
        [InlineProperty, SerializeField] 
        private DamageAttribute _damage;
        [InlineProperty, SerializeField] 
        private AttackRangeAttribute _attackRange;
        [InlineProperty, SerializeField] 
        private AttackSpeedAttribute _attackSpeed;
        [InlineProperty, SerializeField] 
        private AttackCooldownAttribute _attackCooldown;
        [InlineProperty, SerializeField] 
        private DetectionRangeAttribute _detectionRange;
        [InlineProperty, SerializeField] 
        private UnitCostAttribute _unitCost;
        [InlineProperty, SerializeField] 
        private UnitPrefabAttribute _unitPrefab;
        
        
        
    }
}
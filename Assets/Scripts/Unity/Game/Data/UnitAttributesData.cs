using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Game.Attributes;
using Unity.Game.Attributes.Specific;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unity.Game
{
    [CreateAssetMenu(fileName = "UnitData.asset", menuName = "CraftTD/UnitData", order = 1)]

    [Serializable]
    public class UnitAttributesData : ScriptableObject, IGameEntityData
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
        
        public GameEntityAttribute[] GetAllAttributes()
        {
            return new GameEntityAttribute[]
            {
                _health,
                _moveSpeed,
                _damage,
                _attackRange,
                _attackSpeed,
                _attackCooldown,
                _detectionRange,
                _unitCost
            };
        }

        
        public bool TryGetAttribute<T>(out T attribute) where T : GameEntityAttribute
        {
            var allAttributes = GetAllAttributes();
            foreach (var attr in allAttributes)
            {
                if (attr is T typedAttr)
                {
                    attribute = typedAttr;
                    return true;
                }
            }

            attribute = default;
            return false;
        }
        
    }
}
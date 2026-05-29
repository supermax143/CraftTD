using System;
using UnityEngine;

namespace Unity.Game
{
    [Serializable]
    public class UnitAttack
    {
        public enum AttackType
        {
            Melee,
            RangedInstant,
            RangedProjectile
        }
        
        [SerializeField] 
        private float _damage;
        [SerializeField] 
        private float _range;
        [SerializeField]
        private float _speed;
        [SerializeField] 
        private float _cooldown;
        [SerializeField] 
        private AttackType _type;

        public float Damage => _damage;
        public float Range => _range;
        public float Speed => _speed;
        public float Cooldown => _cooldown;
        public AttackType Type => _type;
            
    }
}
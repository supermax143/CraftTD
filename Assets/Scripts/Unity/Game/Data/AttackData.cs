using System;
using UnityEngine;

namespace Unity.Game
{
    [Serializable]
    public class AttackData
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
        [SerializeField]
        private GameObject _projectilePrefab;
        
        public float Damage => _damage;
        public float Range => _range;
        public float Speed => _speed;
        public float Cooldown => _cooldown;
        public AttackType Type => _type;

        
        public AttackData Clone()
        {
            return new AttackData
            {
                _damage = _damage,
                _range = _range,
                _speed = _speed,
                _cooldown = _cooldown,
                _type = _type,
                _projectilePrefab = _projectilePrefab
            };
        }
        
    }
}
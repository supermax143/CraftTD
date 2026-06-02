using System;
using UnityEngine;

namespace Unity.Game
{
    [Serializable]
    public class MoveData
    {
        [SerializeField] 
        private float _speed;
        [SerializeField] 
        private float _rotationSpeed;

        public float Speed => _speed;
        public float RotationSpeed => _rotationSpeed;

        public MoveData Clone()
        {
            return new MoveData
            {
                _speed = _speed,
                _rotationSpeed = _rotationSpeed
            };
        }
    }
}
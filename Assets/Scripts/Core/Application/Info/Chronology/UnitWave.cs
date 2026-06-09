using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unity.Game
{
    [Serializable]
    public class UnitWave
    {
        [ListDrawerSettings(ShowIndexLabels = true)]
        [SerializeField]
        private List<UnitsSquad> _squads;
        
        public List<UnitsSquad> Squads => _squads;
        
        [Serializable]
        public class UnitsSquad
        {
            [SerializeField] 
            private float _delay = 3;
            [SerializeField] 
            private UnitTier _tier;
            [SerializeField] 
            private int _count;
            
            public float Delay => _delay;
            public UnitTier Tier => _tier;
            public int Count => _count;

        }
    }
}
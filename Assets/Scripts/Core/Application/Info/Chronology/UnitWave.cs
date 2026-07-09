using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Game
{
    [Serializable]
    public class UnitWave
    {
        [SerializeField]
        private List<UnitsSquad> _squads;
        
        public List<UnitsSquad> Squads => _squads;
        
        [Serializable]
        public class UnitsSquad
        {
            [SerializeField] 
            private GameStats.UnitSquadDelay _delay = GameStats.UnitSquadDelay.Normal;
            [SerializeField] 
            private UnitTier _tier;
            [SerializeField] 
            private int _count;
            
            public GameStats.UnitSquadDelay Delay => _delay;
            public UnitTier Tier => _tier;
            public int Count => _count;

        }
    }
}
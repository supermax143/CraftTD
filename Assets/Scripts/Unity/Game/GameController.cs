using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class GameController : MonoBehaviour
    {
        
        [SerializeField]
        private List<Team> _teams;
        
        public bool TryGetOpponentTower(Faction opponentFaction,out AttackTarget target)
        {
            target = default;
            var team = _teams.FirstOrDefault(team => team.Tower.Faction == opponentFaction);
            if (team == null)
            {
                return false;
            }
            
            target = team.Tower.AttackTarget;
            return true;
        }
        
        public void StartGame()
        {
            
        }
        
    }
}
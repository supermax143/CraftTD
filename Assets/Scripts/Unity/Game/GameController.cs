using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class GameController : MonoBehaviour
    {
        
        public event Action<Faction> OnTowerDestroyed;
        
        [SerializeField]
        private List<Team> _teams;
        
        [Inject] private ChronologyData _chronologyData;
        
        public void Start()
        {
            var epoch = _chronologyData.Epochs.First();
            foreach (var team in _teams)
            {
                team.Initialize(epoch);
                team.Tower.OnDestroyed += TowerDestroyedHandler;
            }
            
            foreach (var team in _teams)
            {
                team.StartGame();
            }
        }

        private void TowerDestroyedHandler(TowerController tower)
        {
            OnTowerDestroyed?.Invoke(tower.Faction);
        }


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
        
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Application.Interfaces.ApplicationSession;
using UnityEngine;
using Zenject;

namespace Unity.Game
{
    public class GameController : MonoBehaviour, IGameController
    {
        
        public event Action<Faction> OnTowerDestroyed;
        
        [SerializeField]
        private List<Team> _teams;
        [SerializeField] 
        private FoodProduction _foodProduction;

        [Inject] private EpochManager _epochManager;
        [Inject] private IApplicationSession _applicationSession;
        
        private Spawner _spawner;
        private bool _started = false;

        private void Start()
        {
            foreach (var team in _teams)
            {
                team.Initialize();
                team.Tower.OnDestroyed += TowerDestroyedHandler;
                if (team.Faction == Faction.Player)
                {
                    _spawner = team.Spawner;
                }
            }
        }

        public void StartGame()
        {
            if (_started)
            {
                return;
            }
            
            foreach (var team in _teams)
            {
                team.StartGame();
            }
            _foodProduction.StartProduction();
            _started = true;
        }

        private void TowerDestroyedHandler(TowerController tower)
        {
            _foodProduction.StopProduction();
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
        
        public void BuyUnit(UnitTier tier)
        {
            _epochManager.TryGetUnitDataByTier(tier, out var unitData);
            if (_foodProduction.FoodCount < unitData.FoodCost)
            {
                return;
            }
            _foodProduction.WithdrawFood(unitData.FoodCost);
            _spawner.Spawn(tier, 1);
        }

        public void ExitGame()
        {
            _applicationSession.CurrentState.ExitGame();
        }
    }
}
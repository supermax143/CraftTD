using System;
using System.Threading.Tasks;

namespace Unity.Game
{
    public interface IGameController
    {
        void BuyUnit(UnitTier tier);
        event Action<Faction> OnGameFinished;
        bool TryGetOpponentTower(Faction opponentFaction,out AttackTargetBase target);
        void Pause(bool pause);

        void FinishRound(Faction winner);
        void SelectNextEnemyEpoch();
        void SelectPrevEnemyEpoch();
        void BlocUI();
        void UnblockUI();
        Team GetTeam(Faction faction);
        LocationContainer LocationContainer { get; }
        void Reset();
        void StartBattle();
    }
}
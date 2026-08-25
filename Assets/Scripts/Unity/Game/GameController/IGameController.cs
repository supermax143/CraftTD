using System;
using System.Threading.Tasks;

namespace Unity.Game
{
    public interface IGameController
    {
        void BuyUnit(UnitTier tier);
        event Action<Faction> OnGameFinished;
        bool TryGetTower(Faction faction,out AttackTargetBase target);
        void Pause(bool pause);

        void FinishRound(Faction winner, bool force);
        void SelectNextEnemyEpoch();
        void SelectPrevEnemyEpoch();
        void BlocUI();
        void UnblockUI();
        Team GetTeam(Faction faction);
        LocationSwitcher LocationSwitcher { get; }
        void Reset();
        void StartBattle();
    }
}
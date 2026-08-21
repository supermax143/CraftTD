using System;
using System.Threading.Tasks;

namespace Unity.Game
{
    public interface IGameController
    {
        event Action<UnitTier, bool> OnDefenseStanceSwitched;
        event Action<Faction> OnGameFinished;
        void BuyUnit(UnitTier tier);
        bool TryGetTower(Faction faction,out AttackTargetBase target);
        void Pause(bool pause);
        void FinishRound(Faction winner, bool force);
        void SelectNextEnemyEpoch();
        void SelectPrevEnemyEpoch();
        void BlocUI();
        void UnblockUI();
        Team GetTeam(Faction faction);
        LocationContainer LocationContainer { get; }
        void Reset();
        void StartBattle();
        void SwitchDefenseStance(UnitTier tier);
        bool TryGetDefenseStance(UnitTier tier, out bool defenceStanceActive);
    }
}
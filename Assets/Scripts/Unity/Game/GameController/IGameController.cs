using System;

namespace Unity.Game
{
    public interface IGameController
    {
        void BuyUnit(UnitTier tier);
        event Action<Faction> OnTowerDestroyed;
        bool TryGetOpponentTower(Faction opponentFaction,out AttackTarget target);
    }
}
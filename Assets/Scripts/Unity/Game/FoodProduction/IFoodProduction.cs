using System;

namespace Unity.Game
{
    public interface IFoodProduction
    {
        event Action OnFoodProductionStarted;
        event Action OnFoodProduced;
        int CurrentFoodCount { get; }
        float CurProgress { get; }
        bool Started { get; }
    }
}
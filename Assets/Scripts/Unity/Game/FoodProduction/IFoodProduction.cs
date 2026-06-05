using System;

namespace Unity.Game
{
    public interface IFoodProduction
    {
        event Action OnFoodProductionStarted;
        event Action OnFoodChanged;
        int FoodCount { get; }
        float CurProgress { get; }
        bool Started { get; }
    }
}
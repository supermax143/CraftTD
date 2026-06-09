using Unity.Game;

namespace Core.Application.Interfaces.Info
{
    public interface IUnitInfo
    {
        string Name { get; }
        UnitTier Tier { get; }
        int FoodCost { get; }
        int UnlockCost { get; }
    }
}
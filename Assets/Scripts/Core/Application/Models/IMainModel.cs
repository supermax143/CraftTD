
namespace Core.Application.Models
{
    public interface IMainModel 
    {
        EpochModel EnemyEpoch { get; }
        EpochModel PlayerEpoch { get; }
        Resource Money { get; set; }
        int CurrentEpochNumber { get; }
        bool HasNextEpoch();
        void CompleteEpoch();
        int GetEpochCompleteCost();
        void Reset();
    }
    
    internal interface IMainModelInternal : IMainModel
    {
    }
    
}
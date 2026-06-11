
namespace Core.Application.Models
{
    public interface IMainModel 
    {
        EpochModel Epoch { get; }
        uint Money { get; set; }
        int CurrentEpochId { get; }
        bool HasNextEpoch();
        void CompleteEpoch();
        int GetEpochCompleteCost();
        void Reset();
    }
    
    internal interface IMainModelInternal : IMainModel
    {
        
    }
    
}

namespace Core.Application.Models
{
    public interface IMainModel 
    {
        EpochModel Epoch { get; }
        uint Money { get; set; }
    }
    
    internal interface IMainModelInternal : IMainModel
    {
        
    }
    
}
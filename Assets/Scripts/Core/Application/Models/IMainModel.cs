
namespace Core.Application.Models
{
    public interface IMainModel
    {
        uint Money { get; set; }
    }
    
    internal interface IMainModelInternal : IMainModel
    {
    }
    
}
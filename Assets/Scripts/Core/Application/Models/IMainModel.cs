using Core.Domain.Models;

namespace Core.Application.Models
{
    public interface IMainModel
    {
        UserModel User { get; }
    }
    
    internal interface IMainModelInternal : IMainModel
    {
        void SetUser(UserModel user);
    }
    
}
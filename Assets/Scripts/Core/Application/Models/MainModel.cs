using Core.Domain.Models;

namespace Core.Application.Models
{
    internal class MainModel : IMainModelInternal
    {
        public UserModel User { get; private set; }
        
        public void SetUser(UserModel user)
        {
            User = user;
        }
    }
}
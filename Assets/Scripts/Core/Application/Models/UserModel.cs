namespace Core.Domain.Models
{
    public class UserModel
    {
        public int UserId { get;}
        public string UserName { get;}

        public UserModel(int userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }

        public UserModel()
        {
        }
    }
}
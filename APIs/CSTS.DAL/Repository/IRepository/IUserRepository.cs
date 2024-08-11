using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IUserRepository
    {
        User1 GetUserByEmailOrUserName(string emailOrUserName);
        User1 GetUserByEmailOrUserName(object emailOrUserName);
    }
}

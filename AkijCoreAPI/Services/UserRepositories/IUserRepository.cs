using AkijCoreAPI.Models;

namespace AkijCoreAPI.Services.UserRepositories
{
    public interface IUserRespository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetById(int id);
        Task<User> GetByUsername(string username);
        Task<User> CreateUser(User user);
    }
}

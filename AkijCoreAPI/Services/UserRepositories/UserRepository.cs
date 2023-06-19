using AkijCoreAPI.DataContext;
using AkijCoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AkijCoreAPI.Services.UserRepositories
{
    public class UserRepository : IUserRespository
    {
        private readonly APIDbContext dbContext;

        public UserRepository(APIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> CreateUser(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetByEmail(string email)
        {

            return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            
        }

        public async Task<User> GetById(int id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}

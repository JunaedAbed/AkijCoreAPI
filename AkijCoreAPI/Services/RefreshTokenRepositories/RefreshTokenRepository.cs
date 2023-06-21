using AkijCoreAPI.DataContext;
using AkijCoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AkijCoreAPI.Services.RefreshTokenRepositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        public readonly List<RefreshToken> refreshTokens = new List<RefreshToken>();
        private readonly APIDbContext aPIDbContext;

        public RefreshTokenRepository(APIDbContext aPIDbContext)
        {
            this.aPIDbContext = aPIDbContext;
        }

        public async Task Create(RefreshToken refreshToken)
        {
            aPIDbContext.RefreshTokens.Add(refreshToken);
            await aPIDbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            return await aPIDbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task Delete(Guid id)
        {
            RefreshToken refreshToken = await aPIDbContext.RefreshTokens.FindAsync(id);
            if (refreshToken != null)
            {
                aPIDbContext.RefreshTokens.Remove(refreshToken);
                await aPIDbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAll(int id)
        {
            IEnumerable<RefreshToken> refreshTokens = await aPIDbContext.RefreshTokens
               .Where(t => t.Uid == id)
               .ToListAsync();

            aPIDbContext.RefreshTokens.RemoveRange(refreshTokens);
            await aPIDbContext.SaveChangesAsync();
        }
    }
}

using AkijCoreAPI.Models;
using AkijCoreAPI.Models.Responses;
using AkijCoreAPI.Services.RefreshTokenRepositories;
using AkijCoreAPI.Services.TokenGenerators;

namespace AkijCoreAPI.Services.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator accessTokenGenerator;
        private readonly RefreshTokenGenerator refreshTokenGenerator;
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public Authenticator(AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator, IRefreshTokenRepository refreshTokenRepository)
        {
            this.accessTokenGenerator = accessTokenGenerator;
            this.refreshTokenGenerator = refreshTokenGenerator;
            this.refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticatedUserResponse> Authenticate(User user)
        {
            string accessToken = accessTokenGenerator.GenerateToken(user);
            string refreshToken = refreshTokenGenerator.GenerateToken();

            RefreshToken refreshTokenDTO = new RefreshToken()
            {
                Token = refreshToken,
                Uid = user.Id
            };
             await refreshTokenRepository.Create(refreshTokenDTO);

            return new AuthenticatedUserResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}

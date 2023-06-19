using AkijCoreAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AkijCoreAPI.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly AuthenticationConfiguration authenticationConfiguration;
        private readonly TokenGenerator tokenGenerator;

        public RefreshTokenGenerator(AuthenticationConfiguration authenticationConfiguration, TokenGenerator tokenGenerator)
        {
            this.authenticationConfiguration = authenticationConfiguration;
            this.tokenGenerator = tokenGenerator;
        }

        public string GenerateToken()
        {
            return tokenGenerator.GenerateToken(authenticationConfiguration.RefreshTokenSecret, authenticationConfiguration.Issuer, authenticationConfiguration.Audience, authenticationConfiguration.RefreshTokenExpiration); 
        }
    }
}

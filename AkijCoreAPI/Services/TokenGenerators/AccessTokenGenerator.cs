using AkijCoreAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AkijCoreAPI.Services.TokenGenerators
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfiguration authenticationConfiguration;
        private readonly TokenGenerator tokenGenerator;

        public AccessTokenGenerator(AuthenticationConfiguration authenticationConfiguration, TokenGenerator tokenGenerator)
        {
            this.authenticationConfiguration = authenticationConfiguration;
            this.tokenGenerator = tokenGenerator;
        }

        public string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>() { 
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
            }; 
           
            return tokenGenerator.GenerateToken(authenticationConfiguration.AccessTokenSecret, authenticationConfiguration.Issuer, authenticationConfiguration.Audience, authenticationConfiguration.AccessTokenExpiration, claims);
        }
    }
}

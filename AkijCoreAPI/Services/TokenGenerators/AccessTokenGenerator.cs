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

        public AccessTokenGenerator(AuthenticationConfiguration authenticationConfiguration)
        {
            this.authenticationConfiguration = authenticationConfiguration;
        }

        public string GenerateToken(User user)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>() { 
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
            }; 

            JwtSecurityToken token = new JwtSecurityToken(
                authenticationConfiguration.Issuer, 
                authenticationConfiguration.Audience, 
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(authenticationConfiguration.AccessTokenExpiration),
                credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);  
        }
    }
}

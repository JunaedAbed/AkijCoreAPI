using AkijCoreAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AkijCoreAPI.Services.TokenValidators
{
    public class RefreshTokenValidator
    {
        private readonly AuthenticationConfiguration authenticationConfiguration;

        public RefreshTokenValidator(AuthenticationConfiguration authenticationConfiguration)
        {
            this.authenticationConfiguration = authenticationConfiguration;
        }

        public bool Validate(string refreshToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.RefreshTokenSecret)),
                ValidIssuer = authenticationConfiguration.Issuer,
                ValidAudience = authenticationConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            try
            {
                
                handler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {

                return false; 
            }

            

        }
    }
}

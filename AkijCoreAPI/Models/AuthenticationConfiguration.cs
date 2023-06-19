namespace AkijCoreAPI.Models
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenSecret { get; set; }
        public double AccessTokenExpiration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string RefreshTokenSecret { get; set; }
        public double RefreshTokenExpiration { get; set; }
    }
}

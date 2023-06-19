using System.ComponentModel.DataAnnotations;

namespace AkijCoreAPI.Models
{
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        public string Token { get; set; }    
        public int Uid { get; set; }
    }
}

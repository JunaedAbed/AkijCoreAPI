using System.ComponentModel.DataAnnotations;

namespace AkijCoreAPI.Models.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}

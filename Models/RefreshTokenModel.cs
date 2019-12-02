using System.ComponentModel.DataAnnotations;

namespace Staffinfo.Divers.Models
{
    public class RefreshTokenModel
    {
        public int UserId { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}

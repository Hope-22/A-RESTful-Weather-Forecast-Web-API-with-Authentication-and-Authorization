using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Model
{
    public class User
    {   private Guid _id { get; set; }
        [Key]
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } 
        public byte[] PasswordSalt { get; set; }
    }
}

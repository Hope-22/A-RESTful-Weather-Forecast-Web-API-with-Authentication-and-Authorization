namespace WeatherAPI.Model
{
    public class UserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        //public byte[] PasswordHash { get; }
        //public byte[] PasswordSalt { get; }
    }
}
